using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // GET CART BY USER
        // ===============================
        [HttpGet("{userId}")]
        public IActionResult GetCart(string userId)
        {
            var cart = _context.cartItemAPIs
                .Include(c => c.ProductVariant)
                    .ThenInclude(p => p.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    c.CartItemId,
                    c.UserId,
                    c.ProductVariantId,
                    c.Quantity,
                    ProductName = c.ProductVariant.Product.Name
                })
                .ToList();

            return Ok(cart);
        }

        // ===============================
        // ADD TO CART
        // ===============================
        [HttpPost("add")]
        [HttpPost]
        public IActionResult AddToCart(CartItemDTO dto)
        {
            var variant = _context.productVariantAPIs
                .FirstOrDefault(p => p.ProductVariantId == dto.ProductVariantId);

            if (variant == null)
                return BadRequest("Variant không tồn tại");

            var existingItem = _context.cartItemAPIs
                .FirstOrDefault(c =>
                    c.UserId == dto.UserId &&
                    c.ProductVariantId == dto.ProductVariantId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var item = new CartItemAPI
                {
                    UserId = dto.UserId,
                    ProductVariantId = dto.ProductVariantId,
                    Quantity = dto.Quantity
                };

                _context.cartItemAPIs.Add(item);
            }

            _context.SaveChanges();

            return Ok("Added to cart");
        }


        // ===============================
        // UPDATE QUANTITY
        // ===============================
        [HttpPut("update")]
        public IActionResult UpdateQuantity(CartItemAPI dto)
        {
            var item = _context.cartItemAPIs
                .FirstOrDefault(c => c.CartItemId == dto.CartItemId);

            if (item == null)
                return NotFound();

            item.Quantity = dto.Quantity;

            _context.SaveChanges();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // ===============================
        // REMOVE ITEM
        // ===============================
        [HttpDelete("remove/{id}")]
        public IActionResult RemoveItem(int id)
        {
            var item = _context.cartItemAPIs.Find(id);

            if (item == null)
                return NotFound();

            _context.cartItemAPIs.Remove(item);
            _context.SaveChanges();

            return Ok(new { message = "Đã xoá sản phẩm khỏi giỏ" });
        }

        // ===============================
        // CLEAR CART
        // ===============================
        [HttpDelete("clear/{userId}")]
        public IActionResult ClearCart(string userId)
        {
            var items = _context.cartItemAPIs
                .Where(c => c.UserId == userId);

            _context.cartItemAPIs.RemoveRange(items);
            _context.SaveChanges();

            return Ok(new { message = "Đã xoá toàn bộ giỏ hàng" });
        }
    }
}
