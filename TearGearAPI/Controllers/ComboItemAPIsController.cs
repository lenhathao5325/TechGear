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
    public class ComboItemAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComboItemAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ========================================
        // GET ALL
        // ========================================
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await _context.comboItems
                .Include(x => x.Combo)
                .Include(x => x.ProductVariant)
                    .ThenInclude(v => v.Product)
                .ToListAsync();

            return Ok(items);
        }

        // ========================================
        // GET BY ID
        // ========================================
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var item = await _context.comboItems
                .Include(x => x.ProductVariant)
                    .ThenInclude(v => v.Product)
                .FirstOrDefaultAsync(x => x.ComboItemId == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // ========================================
        // GET ITEMS BY COMBO
        // ========================================
        [HttpGet("combo/{comboId}")]
        public async Task<ActionResult> GetByCombo(int comboId)
        {
            var items = await _context.comboItems
                .Where(x => x.ComboId == comboId)
                .Include(x => x.ProductVariant)
                    .ThenInclude(v => v.Product)
                .ToListAsync();

            return Ok(items);
        }

        // ========================================
        // CREATE
        // ========================================
        [HttpPost]
        public async Task<ActionResult> Create(ComboItemAPI model)
        {
            var combo = await _context.comboAPIs.FindAsync(model.ComboId);
            if (combo == null)
                return BadRequest("Combo not found");

            var variant = await _context.productVariantAPIs.FindAsync(model.ProductVariantId);
            if (variant == null)
                return BadRequest("Variant not found");

            _context.comboItems.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // ========================================
        // UPDATE
        // ========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComboItemAPI model)
        {
            if (id != model.ComboItemId)
                return BadRequest();

            var item = await _context.comboItems.FindAsync(id);
            if (item == null)
                return NotFound();

            item.Quantity = model.Quantity;
            item.ProductVariantId = model.ProductVariantId;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // ========================================
        // DELETE
        // ========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.comboItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.comboItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
