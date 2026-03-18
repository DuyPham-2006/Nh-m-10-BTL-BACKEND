using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PaymentsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
        {
            var list = await _db.Payments
                .Include(p => p.WaterBill)
                .Include(p => p.Customer)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetById(int id)
        {
            var payment = await _db.Payments
                .Include(p => p.WaterBill)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
                return NotFound(new { message = "Payment not found" });

            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> Create([FromBody] Payment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.AmountPaid <= 0)
                return BadRequest(new { message = "AmountPaid must be greater than 0" });

            var bill = await _db.WaterBills.FindAsync(model.BillId);
            if (bill == null)
                return BadRequest(new { message = "Bill not found" });

            if (model.AmountPaid > bill.TotalAmount)
                return BadRequest(new { message = "AmountPaid cannot exceed bill total" });

            var customer = await _db.Customers.FindAsync(model.CustomerId);
            if (customer == null)
                return BadRequest(new { message = "Customer not found" });

            model.CreatedAt = DateTime.UtcNow;
            _db.Payments.Add(model);

            if (Math.Abs(model.AmountPaid - bill.TotalAmount) < 0.01m)
            {
                bill.BillStatus = "PAID";
            }

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.PaymentId }, model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Payment>> Update(int id, [FromBody] Payment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _db.Payments.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Payment not found" });

            existing.AmountPaid = model.AmountPaid;
            existing.PaymentMethod = model.PaymentMethod;
            existing.PaymentDate = model.PaymentDate;
            existing.PaymentRefCode = model.PaymentRefCode;
            existing.Notes = model.Notes;
            existing.ConfirmedBy = model.ConfirmedBy;

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _db.Payments.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Payment not found" });

            _db.Payments.Remove(existing);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Payment deleted" });
        }

        [HttpGet("bill/{billId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByBill(int billId)
        {
            var list = await _db.Payments
                .Where(p => p.BillId == billId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetByCustomer(int customerId)
        {
            var list = await _db.Payments
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return Ok(list);
        }
    }
}
