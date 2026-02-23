using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TearGearAPI.DTO;
using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TearGearAPI.Controllers
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

        // GET: api/OrderDetailAPIs - L?y t?t c? chi ti?t ??n hàng
        [HttpGet]
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

        // GET: api/OrderDetailAPIs/5 - L?y chi ti?t ??n hàng theo ID
        [HttpGet("{id}")]
        public IActionResult GetOrderDetailAPI(int id)
        {
            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi ti?t ??n hàng v?i id {id} không t?n t?i" });
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

        // GET: api/OrderDetailAPIs/order/5 - L?y t?t c? chi ti?t c?a m?t ??n hàng
        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetailsByOrderId(int orderId)
        {
            var orderExists = _context.orderAPIs.Any(o => o.OrderId == orderId);
            if (!orderExists)
            {
                return BadRequest(new { error = "??n hàng không t?n t?i" });
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
                return Ok(new { message = "??n hàng này ch?a có chi ti?t s?n ph?m", data = new List<object>() });
            }

            return Ok(data);
        }

        // POST: api/OrderDetailAPIs - Thêm s?n ph?m vào ??n hàng
        // Ch? cho phép thêm khi Order.Status == Pending
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderDetailDTO dto)
        {
            // Validation
            if (dto.Quantity <= 0)
            {
                return BadRequest(new { error = "S? l??ng ph?i l?n h?n 0" });
            }

            if (dto.UnitPrice <= 0)
            {
                return BadRequest(new { error = "Giá s?n ph?m ph?i l?n h?n 0" });
            }

            // Ki?m tra Order t?n t?i
            var order = _context.orderAPIs.FirstOrDefault(o => o.OrderId == dto.OrderId);
            if (order == null)
            {
                return BadRequest(new { error = "??n hàng không t?n t?i" });
            }

            // Ch? cho phép thêm khi Order ? tr?ng thái Pending
            if (order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Ch? có th? thêm s?n ph?m khi ??n hàng ? tr?ng thái Pending. Tr?ng thái hi?n t?i: {order.Status}",
                    currentStatus = order.Status
                });
            }

            // Ki?m tra ProductVariant t?n t?i
            var productVariant = _context.productVariantAPIs
                .Include(pv => pv.Product)
                .FirstOrDefault(pv => pv.Id == dto.ProductVariantId);

            if (productVariant == null)
            {
                return BadRequest(new { error = "Phiên b?n s?n ph?m không t?n t?i" });
            }

            // Ki?m tra stock (n?u c?n)
            if (productVariant.Stock < dto.Quantity)
            {
                return BadRequest(new
                {
                    error = "S? l??ng s?n ph?m không ??",
                    availableStock = productVariant.Stock,
                    requestedQuantity = dto.Quantity
                });
            }

            try
            {
                // Ki?m tra xem s?n ph?m này ?ã có trong ??n hàng ch?a
                var existingDetail = _context.orderDetailAPIs
                    .FirstOrDefault(od => od.OrderId == dto.OrderId && od.ProductVariantId == dto.ProductVariantId);

                if (existingDetail != null)
                {
                    return BadRequest(new { error = "S?n ph?m này ?ã có trong ??n hàng. Hãy c?p nh?t s? l??ng thay vì thêm l?i" });
                }

                var orderDetail = new OrderDetailAPI
                {
                    OrderId = dto.OrderId,
                    ProductVariantId = dto.ProductVariantId,
                    ProductName = productVariant.Product?.Name ?? "Unknown",
                    VariantDescription = productVariant.SKU,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice  // Snapshot giá t?i th?i ?i?m mua
                };

                _context.orderDetailAPIs.Add(orderDetail);

                // C?p nh?t SubTotal và TotalAmount c?a Order
                var totalDetailPrice = _context.orderDetailAPIs
                    .Where(od => od.OrderId == dto.OrderId)
                    .Sum(od => od.Quantity * od.UnitPrice);

                order.SubTotal = totalDetailPrice + (orderDetail.Quantity * orderDetail.UnitPrice);
                order.TotalAmount = order.SubTotal - order.DiscountAmount + order.ShippingFee;

                _context.SaveChanges();

                var response = new
                {
                    message = "Thêm s?n ph?m vào ??n hàng thành công",
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
                    new { error = "L?i khi l?u d? li?u", details = ex.InnerException?.Message });
            }
        }

        // PUT: api/OrderDetailAPIs/5 - C?p nh?t S? L??NG
        // Ch? cho phép c?p nh?t Quantity khi Order.Status == Pending
        // KHÔNG cho phép s?a OrderId, ProductVariantId, ProductName, VariantDescription, UnitPrice
        [HttpPut("{id}")]
        public IActionResult UpdateQuantity(int id, [FromBody] UpdateOrderDetailQuantityDTO dto)
        {
            if (dto.Quantity <= 0)
            {
                return BadRequest(new { error = "S? l??ng ph?i l?n h?n 0" });
            }

            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi ti?t ??n hàng v?i id {id} không t?n t?i" });
            }

            // Ch? cho phép c?p nh?t khi Order ? tr?ng thái Pending
            if (orderDetail.Order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Ch? có th? c?p nh?t s? l??ng khi ??n hàng ? tr?ng thái Pending. Tr?ng thái hi?n t?i: {orderDetail.Order.Status}",
                    currentStatus = orderDetail.Order.Status
                });
            }

            // Ki?m tra stock
            var productVariant = _context.productVariantAPIs.FirstOrDefault(pv => pv.Id == orderDetail.ProductVariantId);
            if (productVariant != null && productVariant.Stock < dto.Quantity)
            {
                return BadRequest(new
                {
                    error = "S? l??ng s?n ph?m không ??",
                    availableStock = productVariant.Stock,
                    requestedQuantity = dto.Quantity
                });
            }

            try
            {
                int quantityDifference = dto.Quantity - orderDetail.Quantity;

                orderDetail.Quantity = dto.Quantity;
                _context.orderDetailAPIs.Update(orderDetail);

                // C?p nh?t SubTotal và TotalAmount c?a Order
                var totalDetailPrice = _context.orderDetailAPIs
                    .Where(od => od.OrderId == orderDetail.OrderId)
                    .Sum(od => (decimal)od.Quantity * od.UnitPrice);

                orderDetail.Order.SubTotal = totalDetailPrice;
                orderDetail.Order.TotalAmount = orderDetail.Order.SubTotal - orderDetail.Order.DiscountAmount + orderDetail.Order.ShippingFee;

                _context.SaveChanges();

                var response = new
                {
                    message = "C?p nh?t s? l??ng s?n ph?m thành công",
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
                    new { error = "L?i khi c?p nh?t d? li?u", details = ex.InnerException?.Message });
            }
        }

        // DELETE: api/OrderDetailAPIs/5 - Xóa chi ti?t ??n hàng
        // Ch? cho phép xóa khi Order.Status == Pending
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderDetail = _context.orderDetailAPIs
                .Include(od => od.Order)
                .FirstOrDefault(od => od.OrderDetailId == id);

            if (orderDetail == null)
            {
                return NotFound(new { error = $"Chi ti?t ??n hàng v?i id {id} không t?n t?i" });
            }

            // Ch? cho phép xóa khi Order ? tr?ng thái Pending
            if (orderDetail.Order.Status != OrderStatus.Pending)
            {
                return BadRequest(new
                {
                    error = $"Ch? có th? xóa chi ti?t khi ??n hàng ? tr?ng thái Pending. Tr?ng thái hi?n t?i: {orderDetail.Order.Status}",
                    currentStatus = orderDetail.Order.Status
                });
            }

            try
            {
                var orderId = orderDetail.OrderId;
                var removedPrice = orderDetail.Quantity * orderDetail.UnitPrice;

                _context.orderDetailAPIs.Remove(orderDetail);

                // C?p nh?t SubTotal và TotalAmount c?a Order
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
                    message = $"Xóa chi ti?t ??n hàng v?i id {id} thành công",
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
                    new { error = "L?i khi xóa d? li?u", details = ex.InnerException?.Message });
            }
        }

        private bool OrderDetailAPIExists(int id)
        {
            return _context.orderDetailAPIs.Any(e => e.OrderDetailId == id);
        }
    }
}
