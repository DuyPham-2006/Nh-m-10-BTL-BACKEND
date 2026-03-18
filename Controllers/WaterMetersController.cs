using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterMetersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WaterMetersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterMeter>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var items = await _db.WaterMeters
                .Include(w => w.Customer)
                .OrderBy(w => w.MeterId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterMeter>> GetById(int id)
        {
            var meter = await _db.WaterMeters
                .Include(w => w.Customer)
                .FirstOrDefaultAsync(w => w.MeterId == id);

            if (meter == null)
                return NotFound(new { message = "Water meter not found" });

            return Ok(meter);
        }

        [HttpPost]
        public async Task<ActionResult<WaterMeter>> Create([FromBody] WaterMeter model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerExists = await _db.Customers.AnyAsync(c => c.CustomerId == model.CustomerId);
            if (!customerExists)
                return BadRequest(new { message = "Customer does not exist" });

            var exists = await _db.WaterMeters.AnyAsync(w => w.MeterCode == model.MeterCode);
            if (exists)
                return Conflict(new { message = "MeterCode already exists" });

            model.CreatedDate = DateTime.UtcNow;
            _db.WaterMeters.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.MeterId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WaterMeter>> Update(int id, [FromBody] WaterMeter model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.WaterMeters.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water meter not found" });

            if (!string.IsNullOrEmpty(model.MeterCode) && model.MeterCode != existing.MeterCode)
            {
                var duplicate = await _db.WaterMeters.AnyAsync(w => w.MeterId != id && w.MeterCode == model.MeterCode);
                if (duplicate)
                    return Conflict(new { message = "MeterCode already exists" });
            }

            existing.CustomerId = model.CustomerId;
            existing.MeterCode = model.MeterCode;
            existing.Brand = model.Brand;
            existing.Model = model.Model;
            existing.InstallDate = model.InstallDate;
            existing.InitialIndex = model.InitialIndex;
            existing.Status = model.Status;
            existing.CreatedDate = model.CreatedDate;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.WaterMeters.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Water meter not found" });

            _db.WaterMeters.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Water meter deleted" });
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<WaterMeter>>> GetByCustomer(int customerId)
        {
            var items = await _db.WaterMeters
                .Where(w => w.CustomerId == customerId)
                .ToListAsync();

            return Ok(items);
        }
    }
}
