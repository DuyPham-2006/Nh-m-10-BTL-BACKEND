using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterSuppliesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WaterSuppliesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterSupply>>> GetAll()
        {
            var list = await _db.WaterSupplies.OrderByDescending(s => s.SupplyMonth).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterSupply>> GetById(int id)
        {
            var supply = await _db.WaterSupplies.FindAsync(id);
            if (supply == null)
                return NotFound(new { message = "Water supply record not found" });

            return Ok(supply);
        }

        [HttpPost]
        public async Task<ActionResult<WaterSupply>> Create([FromBody] WaterSupply model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.TotalM3Pumped <= 0)
                return BadRequest(new { message = "TotalM3Pumped must be greater than 0" });

            var exists = await _db.WaterSupplies.AnyAsync(s => s.SupplyMonth == model.SupplyMonth);
            if (exists)
                return Conflict(new { message = "A supply record already exists for this month" });

            _db.WaterSupplies.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.SupplyId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WaterSupply>> Update(int id, [FromBody] WaterSupply model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.WaterSupplies.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water supply record not found" });

            existing.SupplyMonth = model.SupplyMonth;
            existing.TotalM3Pumped = model.TotalM3Pumped;
            existing.Notes = model.Notes;
            existing.RecordedBy = model.RecordedBy;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.WaterSupplies.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water supply record not found" });

            _db.WaterSupplies.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Water supply record deleted" });
        }
    }
}
