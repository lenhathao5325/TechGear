using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechGearAPI.Data;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.cartItemAPIs
                .Include(c => c.ProductVariant)
                    .ThenInclude(v => v.Product)
                        .ThenInclude(p => p.Images)
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    cartItemId = c.CartItemId,
                    userId = c.UserId,
                    productVariantId = c.ProductVariantId,
                    quantity = c.Quantity,
                    productName = c.ProductVariant.Product.Name,
                    variantDescription = c.ProductVariant.SKU,
                    productImageUrl = c.ProductVariant.Product.Images.FirstOrDefault() != null 
                        ? c.ProductVariant.Product.Images.FirstOrDefault().ImageUrl 
                        : null,
                    unitPrice = c.ProductVariant.Price,
                    availableStock = c.ProductVariant.Stock,
                    totalPrice = c.Quantity * c.ProductVariant.Price
                })
                .ToListAsync();

            return Ok(cartItems);
        }

        // POST: api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if variant exists
            var variant = await _context.productVariantAPIs
                .FirstOrDefaultAsync(v => v.Id == request.ProductVariantId);

            if (variant == null)
                return NotFound(new { message = "Sản phẩm không tồn tại!" });

            // Check stock
            if (variant.Stock < request.Quantity)
                return BadRequest(new { message = "Sản phẩm không đủ số lượng!" });

            // Check if item already in cart
            var existingItem = await _context.cartItemAPIs
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductVariantId == request.ProductVariantId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.Quantity;

                if (existingItem.Quantity > variant.Stock)
                    return BadRequest(new { message = "Sản phẩm không đủ số lượng!" });

                await _context.SaveChangesAsync();
                return Ok(new { message = "Đã cập nhật số lượng trong giỏ hàng!" });
            }

            // Add new item
            var cartItem = new CartItemAPI
            {
                UserId = userId,
                ProductVariantId = request.ProductVariantId,
                Quantity = request.Quantity
            };

            _context.cartItemAPIs.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã thêm vào giỏ hàng!" });
        }

        // PUT: api/cart/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItem = await _context.cartItemAPIs
                .Include(c => c.ProductVariant)
                .FirstOrDefaultAsync(c => c.CartItemId == id && c.UserId == userId);

            if (cartItem == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng!" });

            if (request.Quantity <= 0)
                return BadRequest(new { message = "Số lượng phải lớn hơn 0!" });

            if (request.Quantity > cartItem.ProductVariant.Stock)
                return BadRequest(new { message = "Sản phẩm không đủ số lượng!" });

            cartItem.Quantity = request.Quantity;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật giỏ hàng!" });
        }

        // DELETE: api/cart/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItem = await _context.cartItemAPIs
                .FirstOrDefaultAsync(c => c.CartItemId == id && c.UserId == userId);

            if (cartItem == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng!" });

            _context.cartItemAPIs.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
        }

        // DELETE: api/cart/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.cartItemAPIs
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.cartItemAPIs.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa toàn bộ giỏ hàng!" });
        }
    }

    public class AddToCartRequest
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}
