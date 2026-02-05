using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
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
        public async Task<ActionResult<IEnumerable<PaymentAPI>>> GetpaymentAPIs()
        {
            return await _context.paymentAPIs.ToListAsync();
        }

        // GET: api/PaymentAPIs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentAPI>> GetPaymentAPI(int id)
        {
            var paymentAPI = await _context.paymentAPIs.FindAsync(id);

            if (paymentAPI == null)
            {
                return NotFound();
            }

            return paymentAPI;
        }

        // PUT: api/PaymentAPIs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentAPI(int id, PaymentAPI paymentAPI)
        {
            if (id != paymentAPI.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(paymentAPI).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentAPIExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentAPIs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentAPI>> PostPaymentAPI(PaymentAPI paymentAPI)
        {
            _context.paymentAPIs.Add(paymentAPI);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentAPI", new { id = paymentAPI.PaymentId }, paymentAPI);
        }

        // DELETE: api/PaymentAPIs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentAPI(int id)
        {
            var paymentAPI = await _context.paymentAPIs.FindAsync(id);
            if (paymentAPI == null)
            {
                return NotFound();
            }

            _context.paymentAPIs.Remove(paymentAPI);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentAPIExists(int id)
        {
            return _context.paymentAPIs.Any(e => e.PaymentId == id);
        }
    }
}
