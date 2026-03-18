using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ReportsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("loss")]
        public async Task<ActionResult> GetLossReport([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var suppliesQuery = _db.WaterSupplies.AsQueryable();
            if (from.HasValue)
                suppliesQuery = suppliesQuery.Where(s => s.SupplyMonth >= from.Value);
            if (to.HasValue)
                suppliesQuery = suppliesQuery.Where(s => s.SupplyMonth <= to.Value);

            var supplies = await suppliesQuery.ToListAsync();

            var billsQuery = _db.WaterBills.AsQueryable();
            if (from.HasValue)
                billsQuery = billsQuery.Where(b => b.BillingMonth >= from.Value);
            if (to.HasValue)
                billsQuery = billsQuery.Where(b => b.BillingMonth <= to.Value);

            var bills = await billsQuery.ToListAsync();

            var report = supplies
                .GroupJoin(
                    bills,
                    s => s.SupplyMonth.ToString("yyyy-MM"),
                    b => b.BillingMonth.ToString("yyyy-MM"),
                    (s, bs) => new { Month = s.SupplyMonth.ToString("yyyy-MM"), Supply = s, Bills = bs }
                )
                .Select(g => new
                {
                    report_month = g.Month,
                    total_pumped_m3 = g.Supply.TotalM3Pumped,
                    total_billed_m3 = g.Bills.Sum(b => b.TotalConsumptionM3),
                    loss_m3 = g.Supply.TotalM3Pumped - g.Bills.Sum(b => b.TotalConsumptionM3),
                    loss_percentage = g.Supply.TotalM3Pumped > 0
                        ? Math.Round((g.Supply.TotalM3Pumped - g.Bills.Sum(b => b.TotalConsumptionM3)) / g.Supply.TotalM3Pumped * 100, 2)
                        : 0
                })
                .OrderByDescending(r => r.report_month)
                .ToList();

            return Ok(new { success = true, message = "Loss report", data = report });
        }

        [HttpGet("revenue")]
        public async Task<ActionResult> GetRevenueReport([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
        {
            var billsQuery = _db.WaterBills.AsQueryable();
            if (from.HasValue)
                billsQuery = billsQuery.Where(b => b.BillingMonth >= from.Value);
            if (to.HasValue)
                billsQuery = billsQuery.Where(b => b.BillingMonth <= to.Value);

            var bills = await billsQuery.ToListAsync();

            var report = bills
                .GroupBy(b => b.BillingMonth.ToString("yyyy-MM"))
                .Select(g => new
                {
                    report_month = g.Key,
                    total_billed = g.Sum(b => b.TotalAmount),
                    total_invoices = g.Count(),
                    total_paid = g.Where(b => b.BillStatus == "PAID").Sum(b => b.TotalAmount),
                    total_unpaid = g.Where(b => b.BillStatus != "PAID").Sum(b => b.TotalAmount)
                })
                .OrderByDescending(r => r.report_month)
                .ToList();

            return Ok(new { success = true, message = "Revenue report", data = report });
        }

        [HttpGet("unpaid-bills")]
        public async Task<ActionResult> GetUnpaidBills()
        {
            var bills = await _db.WaterBills
                .Where(b => b.BillStatus == "UNPAID")
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            var total = bills.Sum(b => b.TotalAmount);
            return Ok(new { success = true, message = "Unpaid bills", data = new { total, bills } });
        }

        [HttpGet("overdue-bills")]
        public async Task<ActionResult> GetOverdueBills()
        {
            var today = DateTime.UtcNow.Date;
            var bills = await _db.WaterBills
                .Where(b => b.BillStatus == "UNPAID" && b.DueDate < today)
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            var data = bills.Select(b => new
            {
                b.BillId,
                b.CustomerId,
                b.TotalAmount,
                b.DueDate,
                days_overdue = (today - b.DueDate.Date).Days
            });

            return Ok(new { success = true, message = "Overdue bills", data });
        }
    }
}
