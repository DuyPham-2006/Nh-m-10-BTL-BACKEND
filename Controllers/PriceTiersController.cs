using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceTiersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PriceTiersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterPriceTier>>> GetAll()
        {
            var list = await _db.WaterPriceTiers.OrderBy(p => p.TierLevel).ToListAsync();
            return Ok(list);
        }

        [HttpGet("effective/{date}")]
        public async Task<ActionResult<IEnumerable<WaterPriceTier>>> GetEffective(DateTime date)
        {
            var list = await _db.WaterPriceTiers
                .Where(p => p.EffectiveDate <= date && p.Status == "ACTIVE")
                .OrderBy(p => p.TierLevel)
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<WaterPriceTier>> Create([FromBody] WaterPriceTier model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.FromM3 > model.ToM3)
                return BadRequest(new { message = "FromM3 must be <= ToM3" });

            _db.WaterPriceTiers.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = model.TierId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WaterPriceTier>> Update(int id, [FromBody] WaterPriceTier model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.WaterPriceTiers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Price tier not found" });

            existing.TierLevel = model.TierLevel;
            existing.FromM3 = model.FromM3;
            existing.ToM3 = model.ToM3;
            existing.PricePerM3 = model.PricePerM3;
            existing.EffectiveDate = model.EffectiveDate;
            existing.Status = model.Status;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.WaterPriceTiers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Price tier not found" });

            _db.WaterPriceTiers.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Price tier deleted" });
        }
    }
}
