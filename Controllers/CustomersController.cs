using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CustomersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var query = _db.Customers.AsQueryable();

            var items = await query
                .OrderBy(c => c.CustomerId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Customer not found" });

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] Customer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(model.MeterCode))
            {
                var existing = await _db.Customers.AnyAsync(c => c.MeterCode == model.MeterCode);
                if (existing)
                    return Conflict(new { message = "MeterCode already in use" });
            }

            model.CreatedDate = DateTime.UtcNow;
            _db.Customers.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.CustomerId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> Update(int id, [FromBody] Customer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.Customers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Customer not found" });

            // Prevent changing to a duplicate meter code
            if (!string.IsNullOrEmpty(model.MeterCode) && model.MeterCode != existing.MeterCode)
            {
                var duplicate = await _db.Customers.AnyAsync(c => c.CustomerId != id && c.MeterCode == model.MeterCode);
                if (duplicate)
                    return Conflict(new { message = "MeterCode already in use" });
            }

            existing.FullName = model.FullName;
            existing.Address = model.Address;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Email = model.Email;
            existing.MeterCode = model.MeterCode;
            existing.Status = model.Status;
            existing.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.Customers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Customer not found" });

            // Soft delete: mark inactive
            existing.Status = "INACTIVE";
            existing.UpdatedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Customer marked as inactive" });
        }
    }
}
