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

namespace TearGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderAPIs
        [HttpGet]
        public IActionResult GetorderAPIs()
        {
            var data = _context.orderAPIs
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.ShippingFee,
                    o.UserAddressId,
                    o.DiscountAmount,
                    o.SubTotal,
                    o.CreatedAt
                })
                .ToList();
            return Ok(data);
        }

        // GET: api/OrderAPIs/5
        [HttpGet("{id}")]
        public IActionResult GetOrderAPI(int id)
        {
            var o = _context.orderAPIs
                .Where(o => o.OrderId == id)
                .Select(o => new
                {
                    o.OrderId,
                    o.UserId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.ShippingFee,
                    o.UserAddressId,
                    o.DiscountAmount,
                    o.SubTotal,
                    o.CreatedAt
                })
                .FirstOrDefault();
            if (o == null)
            {
                return NotFound();
            }
            return Ok(o);
        }

        // PUT: api/OrderAPIs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult Update(int id, OrderDTO dto)
        {
            // Find order by OrderId only
            var order = _context.orderAPIs
                .FirstOrDefault(o => o.OrderId == id);
            
            if (order == null)
            {
                return NotFound(new { error = $"Order với id {id} không tồn tại" });
            }

            try
            {
                // Update only the fields provided in the DTO
                order.SubTotal = dto.SubTotal;
                order.ShippingFee = dto.ShippingFee;
                order.DiscountAmount = dto.DiscountAmount;
                order.TotalAmount = dto.TotalAmount;
                order.OrderDate = dto.OrderDate;
                
                // Only update UserId and UserAddressId if they are provided and different
                if (!string.IsNullOrEmpty(dto.UserId) && order.UserId != dto.UserId)
                {
                    var userExists = _context.userAPIs.Any(u => u.Id == dto.UserId);
                    if (!userExists)
                    {
                        return BadRequest(new { error = "UserId không tồn tại trong hệ thống" });
                    }
                    order.UserId = dto.UserId;
                }

                if (dto.UserAddressId.HasValue && order.UserAddressId != dto.UserAddressId)
                {
                    var addressExists = _context.userAddressAPIs.Any(a => a.UserAddressId == dto.UserAddressId);
                    if (!addressExists)
                    {
                        return BadRequest(new { error = "UserAddressId không tồn tại" });
                    }
                    order.UserAddressId = dto.UserAddressId;
                }

                // Parse status if provided
                if (!string.IsNullOrEmpty(dto.Status))
                {
                    Enum.TryParse(dto.Status, out TechGearAPI.Models.Enums.EnumsAPI.OrderStatus status);
                    order.Status = status;
                }

                _context.orderAPIs.Update(order);
                _context.SaveChanges();
                
                // Return only necessary fields (not navigation properties)
                var response = new
                {
                    message = "Order được cập nhật thành công",
                    order = new
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount,
                        Status = order.Status,
                        ShippingFee = order.ShippingFee,
                        UserAddressId = order.UserAddressId,
                        DiscountAmount = order.DiscountAmount,
                        SubTotal = order.SubTotal,
                        CreatedAt = order.CreatedAt
                    }
                };
                
                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "Lỗi khi cập nhật dữ liệu", details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "Lỗi không xác định", details = ex.Message });
            }
        }

        // POST: api/OrderAPIs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult Create(OrderDTO dto)
        {
            // Validate UserId exists
            var userExists = _context.userAPIs.Any(u => u.Id == dto.UserId);
            if (!userExists)
            {
                return BadRequest(new { error = "UserId không tồn tại trong hệ thống" });
            }

            // Validate UserAddressId if provided
            if (dto.UserAddressId.HasValue)
            {
                var addressExists = _context.userAddressAPIs.Any(a => a.UserAddressId == dto.UserAddressId);
                if (!addressExists)
                {
                    return BadRequest(new { error = "UserAddressId không tồn tại" });
                }
            }

            var order = new OrderAPI
            {
                UserId = dto.UserId,
                UserAddressId = dto.UserAddressId,
                SubTotal = dto.SubTotal,
                CreatedAt = dto.CreatedAt,
                ShippingFee = dto.ShippingFee,
                DiscountAmount = dto.DiscountAmount,
                TotalAmount = dto.TotalAmount,
                OrderDate = dto.OrderDate
            };
            Enum.TryParse(dto.Status, out TechGearAPI.Models.Enums.EnumsAPI.OrderStatus status);
            order.Status = status;
            
            try
            {
                _context.orderAPIs.Add(order);
                _context.SaveChanges();
                
                // Return only necessary fields (not navigation properties)
                var response = new
                {
                    message = "Order được tạo thành công",
                    order = new
                    {
                        order.OrderId,
                        order.UserId,
                        order.OrderDate,
                        order.TotalAmount,
                        order.Status,
                        order.ShippingFee,
                        order.UserAddressId,
                        order.DiscountAmount,
                        order.SubTotal,
                        order.CreatedAt
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

        // DELETE: api/OrderAPIs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.orderAPIs
                .FirstOrDefault(o => o.OrderId == id);
            
            if (order == null)
            {
                return NotFound(new { error = $"Order với id {id} không tồn tại" });
            }

            try
            {
                _context.orderAPIs.Remove(order);
                _context.SaveChanges();
                return Ok(new { message = $"Order với id {id} đã được xóa thành công" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "Lỗi khi xóa dữ liệu", details = ex.InnerException?.Message });
            }
        }

        private bool OrderAPIExists(int id)
        {
            return _context.orderAPIs.Any(e => e.OrderId == id);
        }
    }
}
