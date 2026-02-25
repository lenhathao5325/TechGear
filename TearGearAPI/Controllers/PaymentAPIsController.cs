using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PaymentAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentAPIs
        [HttpGet]
        public IActionResult GetPaymentAPIs()
        {
            var data = _context.paymentAPIs
                .Select(p => new
                {
                    p.PaymentId,
                    p.OrderId,
                    p.Amount,
                    p.Method,
                    p.Status,
                    p.TransactionId,
                    p.PaymentDate
                })
                .ToList();
            return Ok(data);
        }

        // GET: api/PaymentAPIs/5
        [HttpGet("{id}")]
        public IActionResult GetPaymentAPI(int id)
        {
            var payment = _context.paymentAPIs
                .Where(p => p.PaymentId == id)
                .Select(p => new
                {
                    p.PaymentId,
                    p.OrderId,
                    p.Amount,
                    p.Method,
                    p.Status,
                    p.TransactionId,
                    p.PaymentDate
                })
                .FirstOrDefault();

            if (payment == null)
            {
                return NotFound(new { error = $"Thanh toán v?i id {id} không t?n t?i" });
            }

            return Ok(payment);
        }

        // PUT: api/PaymentAPIs/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, PaymentDTO dto)
        {
            // Find payment by PaymentId only
            var payment = _context.paymentAPIs
                .Include(p => p.Order)
                .FirstOrDefault(p => p.PaymentId == id);

            if (payment == null)
            {
                return NotFound(new { error = $"Thanh toán v?i id {id} không t?n t?i" });
            }

            // Only allow updating Status and TransactionId
            try
            {
                payment.Status = dto.Status;
                payment.TransactionId = dto.TransactionId;

                // If payment status is Paid (2), automatically update Order status to Confirmed (2)
                if (dto.Status == (int)PaymentStatus.Paid && payment.Order != null)
                {
                    payment.Order.Status = OrderStatus.Confirmed;
                }

                _context.paymentAPIs.Update(payment);
                _context.SaveChanges();

                // Return only necessary fields (not navigation properties)
                var response = new
                {
                    message = "C?p nh?t thanh toán thành công",
                    payment = new
                    {
                        PaymentId = payment.PaymentId,
                        OrderId = payment.OrderId,
                        Amount = payment.Amount,
                        Method = payment.Method,
                        Status = payment.Status,
                        TransactionId = payment.TransactionId,
                        PaymentDate = payment.PaymentDate
                    }
                };

                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "L?i khi c?p nh?t d? li?u", details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "L?i không xác ??nh", details = ex.Message });
            }
        }

        // POST: api/PaymentAPIs
        [HttpPost]
        public IActionResult Create(PaymentDTO dto)
        {
            // Validate OrderId exists
            var order = _context.orderAPIs
                .Include(o => o.Payments)
                .FirstOrDefault(o => o.OrderId == dto.OrderId);

            if (order == null)
            {
                return BadRequest(new { error = "OrderId không t?n t?i trong h? th?ng" });
            }

            // Validate Order status - can only create payment for Pending orders
            if (order.Status != OrderStatus.Pending)
            {
                return BadRequest(new { error = "Ch? có th? t?o thanh toán cho ??n hàng có tr?ng thái Pending" });
            }

            // Check if order already has a successful payment
            var existingSuccessfulPayment = order.Payments
                .Any(p => p.Status == (int)PaymentStatus.Paid);

            if (existingSuccessfulPayment)
            {
                return BadRequest(new { error = "??n hàng này ?ã có thanh toán thành công r?i" });
            }

            var payment = new PaymentAPI
            {
                OrderId = dto.OrderId,
                Amount = order.TotalAmount,  // Use Order.TotalAmount, not from DTO
                Method = dto.Method,
                Status = dto.Status,
                TransactionId = dto.TransactionId,
                PaymentDate = dto.PaymentDate
            };

            try
            {
                _context.paymentAPIs.Add(payment);
                _context.SaveChanges();

                // Return only necessary fields (not navigation properties)
                var response = new
                {
                    message = "T?o thanh toán thành công",
                    payment = new
                    {
                        payment.PaymentId,
                        payment.OrderId,
                        payment.Amount,
                        payment.Method,
                        payment.Status,
                        payment.TransactionId,
                        payment.PaymentDate
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

        // DELETE: api/PaymentAPIs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var payment = _context.paymentAPIs
                .FirstOrDefault(p => p.PaymentId == id);

            if (payment == null)
            {
                return NotFound(new { error = $"Thanh toán v?i id {id} không t?n t?i" });
            }

            // Do not allow deletion of payments
            return BadRequest(new { error = "Không cho phép xóa thanh toán. Vui lòng c?p nh?t tr?ng thái thanh toán thay th?" });
        }

        private bool PaymentAPIExists(int id)
        {
            return _context.paymentAPIs.Any(e => e.PaymentId == id);
        }
    }
}
