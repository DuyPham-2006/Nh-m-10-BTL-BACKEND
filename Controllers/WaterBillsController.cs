using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterBillsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WaterBillsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaterBill>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _db.WaterBills
                .Include(b => b.Customer)
                .Include(b => b.WaterMeter)
                .Include(b => b.WaterReading)
                .OrderByDescending(b => b.BillingMonth)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WaterBill>> GetById(int id)
        {
            var bill = await _db.WaterBills
                .Include(b => b.Customer)
                .Include(b => b.WaterMeter)
                .Include(b => b.WaterReading)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null)
                return NotFound(new { message = "Bill not found" });

            return Ok(bill);
        }

        [HttpPost("generate")]
        public async Task<ActionResult> GenerateBill([FromBody] GenerateBillRequest request)
        {
            var reading = await _db.WaterReadings
                .Include(r => r.WaterMeter)
                .FirstOrDefaultAsync(r => r.ReadingId == request.ReadingId);

            if (reading == null)
                return NotFound(new { message = "Reading not found" });

            if (reading.Status == "INVOICED")
                return BadRequest(new { message = "Reading already invoiced" });

            var customer = await _db.Customers.FindAsync(reading.WaterMeter?.CustomerId);
            if (customer == null)
                return BadRequest(new { message = "Customer not found for reading" });

            var tiers = await _db.WaterPriceTiers
                .Where(t => t.EffectiveDate <= DateTime.UtcNow && t.Status == "ACTIVE")
                .OrderBy(t => t.TierLevel)
                .ToListAsync();

            if (!tiers.Any())
                return BadRequest(new { message = "No active price tiers found" });

            var billDetails = CalculateBillDetails(reading.ConsumptionM3, tiers);

            var dueDate = DateTime.UtcNow.AddDays(15);

            var bill = new WaterBill
            {
                ReadingId = reading.ReadingId,
                CustomerId = customer.CustomerId,
                MeterId = reading.MeterId,
                BillingMonth = reading.ReadingMonth,
                TotalConsumptionM3 = reading.ConsumptionM3,
                SubtotalAmount = billDetails.Subtotal,
                TaxAmount = billDetails.Tax,
                EnvironmentalFee = billDetails.EnvironmentalFee,
                TotalAmount = billDetails.Total,
                BillStatus = "UNPAID",
                BillDate = DateTime.UtcNow,
                DueDate = dueDate,
                ProcessedBy = request.ProcessedBy,
                CreatedAt = DateTime.UtcNow
            };

            _db.WaterBills.Add(bill);
            reading.Status = "INVOICED";

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = bill.BillId }, new
            {
                bill.BillId,
                bill.CustomerId,
                bill.MeterId,
                bill.BillingMonth,
                bill.TotalConsumptionM3,
                bill.SubtotalAmount,
                bill.TaxAmount,
                bill.EnvironmentalFee,
                bill.TotalAmount,
                bill.BillStatus,
                bill.DueDate,
                Details = billDetails.Items
            });
        }

        private BillCalculationResult CalculateBillDetails(decimal consumption, List<WaterPriceTier> tiers)
        {
            var remaining = consumption;
            decimal subtotal = 0;
            var items = new List<BillDetailItem>();

            foreach (var tier in tiers)
            {
                if (remaining <= 0) break;

                var tierVolume = tier.ToM3 - tier.FromM3;
                if (tierVolume <= 0) continue;

                var used = Math.Min(remaining, tierVolume);
                var amount = used * tier.PricePerM3;

                items.Add(new BillDetailItem
                {
                    Tier = tier.TierLevel,
                    FromM3 = tier.FromM3,
                    ToM3 = tier.ToM3,
                    Quantity = used,
                    PricePerM3 = tier.PricePerM3,
                    Amount = amount
                });

                subtotal += amount;
                remaining -= used;
            }

            var tax = subtotal * 0.10m;
            var environmentalFee = 5000m;
            var total = subtotal + tax + environmentalFee;

            return new BillCalculationResult
            {
                Items = items,
                Subtotal = subtotal,
                Tax = tax,
                EnvironmentalFee = environmentalFee,
                Total = total
            };
        }

        public class GenerateBillRequest
        {
            public int ReadingId { get; set; }
            public int? ProcessedBy { get; set; }
        }

        private class BillCalculationResult
        {
            public List<BillDetailItem> Items { get; set; } = new List<BillDetailItem>();
            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }
            public decimal EnvironmentalFee { get; set; }
            public decimal Total { get; set; }
        }

        private class BillDetailItem
        {
            public int Tier { get; set; }
            public decimal FromM3 { get; set; }
            public decimal ToM3 { get; set; }
            public decimal Quantity { get; set; }
            public decimal PricePerM3 { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
