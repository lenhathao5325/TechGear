using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TearGearAPI.DTO;
using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TearGearAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== GET =====================

        // GET: api/orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _context.orderAPIs
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.OrderDate,
                    o.Status,
                    o.SubTotal,
                    o.ShippingFee,
                    o.DiscountAmount,
                    o.TotalAmount
                })
                .ToList();

            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.orderAPIs
                .Where(o => o.OrderId == id)
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.OrderDate,
                    o.Status,
                    o.SubTotal,
                    o.ShippingFee,
                    o.DiscountAmount,
                    o.TotalAmount
                })
                .FirstOrDefault();

            if (order == null)
                return NotFound(new { error = "Đơn hàng không tồn tại" });

            return Ok(order);
        }

        // ===================== CREATE =====================

        // POST: api/orders
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDTO dto)
        {
            // Validate user
            if (!_context.userAPIs.Any(u => u.Id == dto.UserId))
                return BadRequest(new { error = "User không tồn tại" });

            // Validate address
            if (!_context.userAddressAPIs.Any(a => a.UserAddressId == dto.UserAddressId))
                return BadRequest(new { error = "Địa chỉ không tồn tại" });

            var order = new OrderAPI
            {
                UserId = dto.UserId,
                UserAddressId = dto.UserAddressId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                SubTotal = 0,
                ShippingFee = 0,
                DiscountAmount = 0,
                TotalAmount = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.orderAPIs.Add(order);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Tạo đơn hàng thành công",
                order.OrderId
            });
        }

        // ===================== UPDATE STATUS =====================

        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, UpdateOrderStatusDTO dto)
        {
            var order = _context.orderAPIs.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
                return NotFound(new { error = "Đơn hàng không tồn tại" });

            if (!Enum.TryParse(dto.Status, out OrderStatus newStatus))
                return BadRequest(new { error = "Trạng thái không hợp lệ" });

            // Validate flow
            if (!IsValidStatusFlow(order.Status, newStatus))
                return BadRequest(new { error = "Không thể chuyển trạng thái này" });

            order.Status = newStatus;
            _context.SaveChanges();

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }

        // ===================== CANCEL =====================

        // PUT: api/orders/{id}/cancel
        [HttpPut("{id}/cancel")]
        public IActionResult CancelOrder(int id)
        {
            var order = _context.orderAPIs.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
                return NotFound(new { error = "Đơn hàng không tồn tại" });

            if (order.Status != OrderStatus.Pending)
                return BadRequest(new { error = "Chỉ được hủy đơn khi đang chờ xử lý" });

            order.Status = OrderStatus.Cancelled;
            _context.SaveChanges();

            return Ok(new { message = "Hủy đơn hàng thành công" });
        }

        // ===================== PRIVATE =====================

        private bool IsValidStatusFlow(OrderStatus current, OrderStatus next)
        {
            return current switch
            {
                OrderStatus.Pending => next == OrderStatus.Confirmed || next == OrderStatus.Cancelled,
                OrderStatus.Confirmed => next == OrderStatus.Shipping,
                OrderStatus.Shipping => next == OrderStatus.Completed,
                _ => false
            };
        }
    }
}
