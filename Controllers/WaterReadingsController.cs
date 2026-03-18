using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterReadingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WaterReadingsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterReading>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _db.WaterReadings
                .Include(r => r.WaterMeter)
                .OrderByDescending(r => r.ReadingMonth)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterReading>> GetById(int id)
        {
            var reading = await _db.WaterReadings
                .Include(r => r.WaterMeter)
                .FirstOrDefaultAsync(r => r.ReadingId == id);

            if (reading == null)
                return NotFound(new { message = "Water reading not found" });

            return Ok(reading);
        }

        [HttpPost]
        public async Task<ActionResult<WaterReading>> Create([FromBody] WaterReading model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.NewIndex < model.OldIndex)
                return BadRequest(new { message = "NewIndex must be greater than or equal to OldIndex" });

            var meterExists = await _db.WaterMeters.AnyAsync(m => m.MeterId == model.MeterId);
            if (!meterExists)
                return BadRequest(new { message = "Water meter does not exist" });

            var exists = await _db.WaterReadings.AnyAsync(r => r.MeterId == model.MeterId && r.ReadingMonth == model.ReadingMonth);
            if (exists)
                return Conflict(new { message = "Reading for this meter and month already exists" });

            model.ReadingDate = DateTime.UtcNow;
            model.Status = "PENDING";

            _db.WaterReadings.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.ReadingId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WaterReading>> Update(int id, [FromBody] WaterReading model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.WaterReadings.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water reading not found" });

            if (model.NewIndex < model.OldIndex)
                return BadRequest(new { message = "NewIndex must be greater than or equal to OldIndex" });

            existing.OldIndex = model.OldIndex;
            existing.NewIndex = model.NewIndex;
            existing.ReadingMonth = model.ReadingMonth;
            existing.Status = model.Status;
            existing.Notes = model.Notes;
            existing.ReadingStaffId = model.ReadingStaffId;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.WaterReadings.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water reading not found" });

            _db.WaterReadings.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Water reading deleted" });
        }

        [HttpPost("{id}/confirm")]
        public async Task<ActionResult> Confirm(int id)
        {
            var existing = await _db.WaterReadings.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water reading not found" });

            existing.Status = "CONFIRMED";
            await _db.SaveChangesAsync();
            return Ok(new { message = "Water reading confirmed" });
        }
    }
}
