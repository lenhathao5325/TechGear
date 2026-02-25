using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetailAPIs - Lấy tất cả chi tiết đơn hàng
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetOrderDetailAPIs()
        {
            var data = _context.orderDetailAPIs
                .Select(od => new
                {
                    od.OrderDetailId,
                    od.OrderId,
                    od.ProductVariantId,
                    od.ProductName,
                    od.VariantDescription,
                    od.Quantity,
                    od.UnitPrice
                })
                .ToList();
            return Ok(data);
        }

        // GET: api/OrderDetailAPIs/5 - Lấy chi tiết đơn hàng theo ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetOrderDetailAPI(int id)
        {
            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi tiết đơn hàng với id {id} không tồn tại" });
            }

            var response = new
            {
                orderDetail.OrderDetailId,
                orderDetail.OrderId,
                orderDetail.ProductVariantId,
                orderDetail.ProductName,
                orderDetail.VariantDescription,
                orderDetail.Quantity,
                orderDetail.UnitPrice,
                OrderStatus = orderDetail.Order?.Status
            };

            return Ok(response);
        }

        // GET: api/OrderDetailAPIs/order/5 - Lấy tất cả chi tiết của một đơn hàng
        [HttpGet("order/{orderId}")]
        [AllowAnonymous]
        public IActionResult GetOrderDetailsByOrderId(int orderId)
        {
            var orderExists = _context.orderAPIs.Any(o => o.OrderId == orderId);
            if (!orderExists)
            {
                return BadRequest(new { error = "Đơn hàng không tồn tại" });
            }

            var data = _context.orderDetailAPIs
                .Where(od => od.OrderId == orderId)
                .Select(od => new
                {
                    od.OrderDetailId,
                    od.OrderId,
                    od.ProductVariantId,
                    od.ProductName,
                    od.VariantDescription,
                    od.Quantity,
                    od.UnitPrice,
                    TotalPrice = od.Quantity * od.UnitPrice
                })
                .ToList();

            if (!data.Any())
            {
                return Ok(new { message = "Đơn hàng này chưa có chi tiết sản phẩm", data = new List<object>() });
            }

            return Ok(data);
        }

        // POST: api/OrderDetailAPIs - Thêm sản phẩm vào đơn hàng
        // Chỉ cho phép thêm khi Order.Status == Pending
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] CreateOrderDetailDTO dto)
        {
            // Validation
            if (dto.Quantity <= 0)
            {
                return BadRequest(new { error = "Số lượng phải lớn hơn 0" });
            }

            if (dto.UnitPrice <= 0)
            {
                return BadRequest(new { error = "Giá sản phẩm phải lớn hơn 0" });
            }

            // Kiểm tra Order tồn tại
            var order = _context.orderAPIs.FirstOrDefault(o => o.OrderId == dto.OrderId);
            if (order == null)
            {
                return BadRequest(new { error = "Đơn hàng không tồn tại" });
            }

            // Chỉ cho phép thêm khi Order ở trạng thái Pending
            if (order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Chỉ có thể thêm sản phẩm khi đơn hàng ở trạng thái Pending. Trạng thái hiện tại: {order.Status}",
                    currentStatus = order.Status
                });
            }

            // Kiểm tra ProductVariant tồn tại
            var productVariant = _context.productVariantAPIs
                .Include(pv => pv.Product)
                .FirstOrDefault(pv => pv.Id == dto.ProductVariantId);

            if (productVariant == null)
            {
                return BadRequest(new { error = "Phiên bản sản phẩm không tồn tại" });
            }

            // Kiểm tra stock (nếu cần)
            if (productVariant.Stock < dto.Quantity)
            {
                return BadRequest(new
                {
                    error = "Số lượng sản phẩm không đủ",
                    availableStock = productVariant.Stock,
                    requestedQuantity = dto.Quantity
                });
            }

            try
            {
                // Kiểm tra xem sản phẩm này đã có trong đơn hàng chưa
                var existingDetail = _context.orderDetailAPIs
                    .FirstOrDefault(od => od.OrderId == dto.OrderId && od.ProductVariantId == dto.ProductVariantId);

                if (existingDetail != null)
                {
                    return BadRequest(new { error = "Sản phẩm này đã có trong đơn hàng. Hãy cập nhật số lượng thay vì thêm lại" });
                }

                var orderDetail = new OrderDetailAPI
                {
                    OrderId = dto.OrderId,
                    ProductVariantId = dto.ProductVariantId,
                    ProductName = productVariant.Product?.Name ?? "Unknown",
                    VariantDescription = productVariant.SKU,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice  // Snapshot giá tại thời điểm mua
                };

                _context.orderDetailAPIs.Add(orderDetail);

                // Cập nhật SubTotal và TotalAmount của Order
                var totalDetailPrice = _context.orderDetailAPIs
                    .Where(od => od.OrderId == dto.OrderId)
                    .Sum(od => od.Quantity * od.UnitPrice);

                order.SubTotal = totalDetailPrice + (orderDetail.Quantity * orderDetail.UnitPrice);
                order.TotalAmount = order.SubTotal - order.DiscountAmount + order.ShippingFee;

                _context.SaveChanges();

                var response = new
                {
                    message = "Thêm sản phẩm vào đơn hàng thành công",
                    orderDetail = new
                    {
                        orderDetail.OrderDetailId,
                        orderDetail.OrderId,
                        orderDetail.ProductVariantId,
                        orderDetail.ProductName,
                        orderDetail.VariantDescription,
                        orderDetail.Quantity,
                        orderDetail.UnitPrice,
                        TotalPrice = orderDetail.Quantity * orderDetail.UnitPrice
                    }
                };
                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Lỗi khi lưu dữ liệu", details = ex.InnerException?.Message });
            }
        }

        // PUT: api/OrderDetailAPIs/5 - Cập nhật Số Lượng
        // Chỉ cho phép cập nhật Quantity khi Order.Status == Pending
        // KHÔNG cho phép sửa OrderId, ProductVariantId, ProductName, VariantDescription, UnitPrice
        [HttpPut("{id}")]
        [AllowAnonymous]
        public IActionResult UpdateQuantity(int id, [FromBody] UpdateOrderDetailQuantityDTO dto)
        {
            if (dto.Quantity <= 0)
            {
                return BadRequest(new { error = "Số lượng phải lớn hơn 0" });
            }

            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi tiết đơn hàng với id {id} không tồn tại" });
            }

            // Chỉ cho phép cập nhật khi Order ở trạng thái Pending
            if (orderDetail.Order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Chỉ có thể cập nhật số lượng khi đơn hàng ở trạng thái Pending. Trạng thái hiện tại: {orderDetail.Order.Status}",
                    currentStatus = orderDetail.Order.Status
                });
            }

            // Kiểm tra stock
            var productVariant = _context.productVariantAPIs.FirstOrDefault(pv => pv.Id == orderDetail.ProductVariantId);
            if (productVariant != null && productVariant.Stock < dto.Quantity)
            {
                return BadRequest(new
                {
                    error = "Số lượng sản phẩm không đủ",
                    availableStock = productVariant.Stock,
                    requestedQuantity = dto.Quantity
                });
            }

            try
            {
                int quantityDifference = dto.Quantity - orderDetail.Quantity;

                orderDetail.Quantity = dto.Quantity;
                _context.orderDetailAPIs.Update(orderDetail);

                // Cập nhật SubTotal và TotalAmount của Order
                var totalDetailPrice = _context.orderDetailAPIs
                    .Where(od => od.OrderId == orderDetail.OrderId)
                    .Sum(od => (decimal)od.Quantity * od.UnitPrice);

                orderDetail.Order.SubTotal = totalDetailPrice;
                orderDetail.Order.TotalAmount = orderDetail.Order.SubTotal - orderDetail.Order.DiscountAmount + orderDetail.Order.ShippingFee;

                _context.SaveChanges();

                var response = new
                {
                    message = "Cập nhật số lượng sản phẩm thành công",
                    orderDetail = new
                    {
                        orderDetail.OrderDetailId,
                        orderDetail.OrderId,
                        orderDetail.ProductVariantId,
                        orderDetail.ProductName,
                        orderDetail.VariantDescription,
                        orderDetail.Quantity,
                        orderDetail.UnitPrice,
                        TotalPrice = orderDetail.Quantity * orderDetail.UnitPrice
                    },
                    updatedOrder = new
                    {
                        orderDetail.Order.OrderId,
                        orderDetail.Order.SubTotal,
                        orderDetail.Order.TotalAmount,
                        orderDetail.Order.Status
                    }
                };

                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Lỗi khi cập nhật dữ liệu", details = ex.InnerException?.Message });
            }
        }

        // DELETE: api/OrderDetailAPIs/5 - Xóa chi tiết đơn hàng
        // Chỉ cho phép xóa khi Order.Status == Pending
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi tiết đơn hàng với id {id} không tồn tại" });
            }

            // Chỉ cho phép xóa khi Order ở trạng thái Pending
            if (orderDetail.Order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Chỉ có thể xóa chi tiết khi đơn hàng ở trạng thái Pending. Trạng thái hiện tại: {orderDetail.Order.Status}",
                    currentStatus = orderDetail.Order.Status
                });
            }

            try
            {
                var orderId = orderDetail.OrderId;
                var removedPrice = orderDetail.Quantity * orderDetail.UnitPrice;

                _context.orderDetailAPIs.Remove(orderDetail);

                // Cập nhật SubTotal và TotalAmount của Order
                var totalDetailPrice = _context.orderDetailAPIs
                    .Where(od => od.OrderId == orderId)
                    .Sum(od => (decimal)od.Quantity * od.UnitPrice);

                var order = _context.orderAPIs.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    order.SubTotal = totalDetailPrice;
                    order.TotalAmount = order.SubTotal - order.DiscountAmount + order.ShippingFee;
                }

                _context.SaveChanges();

                return Ok(new
                {
                    message = $"Xóa chi tiết đơn hàng với id {id} thành công",
                    removedPrice = removedPrice,
                    updatedOrder = order != null ? new
                    {
                        order.OrderId,
                        order.SubTotal,
                        order.TotalAmount,
                        order.Status
                    } : null
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "Lỗi khi xóa dữ liệu", details = ex.InnerException?.Message });
            }
        }

        private bool OrderDetailAPIExists(int id)
        {
            return _context.orderDetailAPIs.Any(e => e.OrderDetailId == id);
        }
    }
}
