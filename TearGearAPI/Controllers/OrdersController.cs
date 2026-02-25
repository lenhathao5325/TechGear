using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechGearAPI.Constants;
using TechGearAPI.Data;
using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== PUBLIC ENDPOINTS FOR TESTING =====================
        
        // GET: api/orders/public - Lấy tất cả đơn hàng (không cần authentication)
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOrdersPublic()
        {
            var orders = await _context.orderAPIs
                .Include(o => o.User)
                .Include(o => o.UserAddress)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    userId = o.UserId,
                    userName = o.User.FullName,
                    userEmail = o.User.Email,
                    totalAmount = o.TotalAmount,
                    status = o.Status,
                    statusText = GetStatusText(o.Status),
                    orderDate = o.OrderDate,
                    itemCount = o.OrderDetails.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/public/{id} - Lấy đơn hàng theo ID (không cần authentication)
        [HttpGet("public/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderPublic(int id)
        {
            var order = await _context.orderAPIs
                .Include(o => o.User)
                .Include(o => o.UserAddress)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });

            var result = new
            {
                orderId = order.OrderId,
                userId = order.UserId,
                userName = order.User.FullName,
                userEmail = order.User.Email,
                userAddressId = order.UserAddressId,
                shippingAddress = order.UserAddress != null
                    ? $"{order.UserAddress.AddressLine}, {order.UserAddress.Ward}, {order.UserAddress.District}, {order.UserAddress.Province}"
                    : null,
                subTotal = order.SubTotal,
                shippingFee = order.ShippingFee,
                discountAmount = order.DiscountAmount,
                totalAmount = order.TotalAmount,
                status = order.Status,
                statusText = GetStatusText(order.Status),
                orderDate = order.OrderDate,
                createdAt = order.CreatedAt,
                orderDetails = order.OrderDetails.Select(od => new
                {
                    orderDetailId = od.OrderDetailId,
                    productName = od.ProductName,
                    variantDescription = od.VariantDescription,
                    productImageUrl = od.ProductVariant?.Product?.Images?.FirstOrDefault()?.ImageUrl,
                    quantity = od.Quantity,
                    unitPrice = od.UnitPrice,
                    totalPrice = od.Quantity * od.UnitPrice
                })
            };

            return Ok(result);
        }

        // PUT: api/orders/public/{id}/status - Cập nhật trạng thái đơn hàng (không cần authentication - CHỈ ĐỂ TEST)
        [HttpPut("public/{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrderStatusPublic(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _context.orderAPIs.FindAsync(id);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });

            // Validate status transition
            if (!IsValidStatusTransition(order.Status, request.Status))
                return BadRequest(new { message = "Không thể chuyển trạng thái đơn hàng!", currentStatus = order.Status, requestedStatus = request.Status });

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công!", orderId = order.OrderId, status = order.Status, statusText = GetStatusText(order.Status) });
        }

        // ===================== AUTHENTICATED ENDPOINTS =====================

        // GET: api/orders/my-orders
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.orderAPIs
                .Include(o => o.User)
                .Include(o => o.UserAddress)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    userId = o.UserId,
                    userName = o.User.FullName,
                    userEmail = o.User.Email,
                    userAddressId = o.UserAddressId,
                    shippingAddress = o.UserAddress != null 
                        ? $"{o.UserAddress.AddressLine}, {o.UserAddress.Ward}, {o.UserAddress.District}, {o.UserAddress.Province}" 
                        : null,
                    subTotal = o.SubTotal,
                    shippingFee = o.ShippingFee,
                    discountAmount = o.DiscountAmount,
                    totalAmount = o.TotalAmount,
                    status = o.Status,
                    statusText = GetStatusText(o.Status),
                    orderDate = o.OrderDate,
                    createdAt = o.CreatedAt,
                    orderDetails = o.OrderDetails.Select(od => new
                    {
                        orderDetailId = od.OrderDetailId,
                        productName = od.ProductName,
                        variantDescription = od.VariantDescription,
                        quantity = od.Quantity,
                        unitPrice = od.UnitPrice
                    })
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var order = await _context.orderAPIs
                .Include(o => o.User)
                .Include(o => o.UserAddress)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });

            // Check authorization: user can only see their own orders, admin/staff can see all
            if (order.UserId != userId && userRole != RoleConstants.Admin && userRole != RoleConstants.Staff)
                return Forbid();

            var result = new
            {
                orderId = order.OrderId,
                userId = order.UserId,
                userName = order.User.FullName,
                userEmail = order.User.Email,
                userAddressId = order.UserAddressId,
                shippingAddress = order.UserAddress != null
                    ? $"{order.UserAddress.AddressLine}, {order.UserAddress.Ward}, {order.UserAddress.District}, {order.UserAddress.Province}"
                    : null,
                subTotal = order.SubTotal,
                shippingFee = order.ShippingFee,
                discountAmount = order.DiscountAmount,
                totalAmount = order.TotalAmount,
                status = order.Status,
                statusText = GetStatusText(order.Status),
                orderDate = order.OrderDate,
                createdAt = order.CreatedAt,
                orderDetails = order.OrderDetails.Select(od => new
                {
                    orderDetailId = od.OrderDetailId,
                    productName = od.ProductName,
                    variantDescription = od.VariantDescription,
                    productImageUrl = od.ProductVariant.Product.Images.FirstOrDefault()?.ImageUrl,
                    quantity = od.Quantity,
                    unitPrice = od.UnitPrice,
                    totalPrice = od.Quantity * od.UnitPrice
                })
            };

            return Ok(result);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get cart items
            var cartItems = await _context.cartItemAPIs
                .Include(c => c.ProductVariant)
                    .ThenInclude(v => v.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest(new { message = "Giỏ hàng trống!" });

            // Validate address
            var address = await _context.userAddressAPIs
                .FirstOrDefaultAsync(a => a.UserAddressId == request.UserAddressId && a.UserId == userId);

            if (address == null)
                return BadRequest(new { message = "Địa chỉ không hợp lệ!" });

            // Calculate totals
            decimal subTotal = cartItems.Sum(c => c.Quantity * c.ProductVariant.Price);
            decimal shippingFee = 30000; // Fixed shipping fee
            decimal discountAmount = 0;
            decimal totalAmount = subTotal + shippingFee - discountAmount;

            // Create order
            var order = new OrderAPI
            {
                UserId = userId,
                UserAddressId = request.UserAddressId,
                SubTotal = subTotal,
                ShippingFee = shippingFee,
                DiscountAmount = discountAmount,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _context.orderAPIs.Add(order);
            await _context.SaveChangesAsync();

            // Create order details
            foreach (var cartItem in cartItems)
            {
                var orderDetail = new OrderDetailAPI
                {
                    OrderId = order.OrderId,
                    ProductVariantId = cartItem.ProductVariantId,
                    ProductName = cartItem.ProductVariant.Product.Name,
                    VariantDescription = cartItem.ProductVariant.SKU,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.ProductVariant.Price
                };

                _context.orderDetailAPIs.Add(orderDetail);

                // Update stock
                cartItem.ProductVariant.Stock -= cartItem.Quantity;
            }

            // Clear cart
            _context.cartItemAPIs.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt hàng thành công!", orderId = order.OrderId });
        }

        // PUT: api/orders/{id}/status (Admin only)
        [HttpPut("{id}/status")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _context.orderAPIs.FindAsync(id);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });

            // Validate status transition
            if (!IsValidStatusTransition(order.Status, request.Status))
                return BadRequest(new { message = "Không thể chuyển trạng thái đơn hàng!" });

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công!", status = order.Status });
        }

        // GET: api/orders (Admin only)
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.orderAPIs
                .Include(o => o.User)
                .Include(o => o.UserAddress)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    orderId = o.OrderId,
                    userId = o.UserId,
                    userName = o.User.FullName,
                    userEmail = o.User.Email,
                    totalAmount = o.TotalAmount,
                    status = o.Status,
                    statusText = GetStatusText(o.Status),
                    orderDate = o.OrderDate,
                    itemCount = o.OrderDetails.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        private static string GetStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Confirmed => "Đã xác nhận",
                OrderStatus.Shipping => "Đang giao",
                OrderStatus.Completed => "Hoàn tất",
                OrderStatus.Cancelled => "Đã hủy",
                _ => "Không xác định"
            };
        }

        private static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Shipping) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Shipping, OrderStatus.Completed) => true,
                _ => false
            };
        }
    }

    public class CreateOrderRequest
    {
        public int UserAddressId { get; set; }
        public string PaymentMethod { get; set; } = "COD";
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
