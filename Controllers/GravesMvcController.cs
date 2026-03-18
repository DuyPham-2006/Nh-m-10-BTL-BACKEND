using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    public class GravesMvcController : Controller
    {
        private readonly AppDbContext _db;

        public GravesMvcController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var graves = await _db.Graves.OrderBy(g => g.GraveId).ToListAsync();
            return View(graves);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grave model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedDate = DateTime.UtcNow;
            _db.Graves.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var grave = await _db.Graves.FindAsync(id);
            if (grave == null)
                return NotFound();

            return View(grave);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grave model)
        {
            if (id != model.GraveId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var existing = await _db.Graves.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Area = model.Area;
            existing.RowNumber = model.RowNumber;
            existing.GraveNumber = model.GraveNumber;
            existing.Location = model.Location;
            existing.Status = model.Status;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var grave = await _db.Graves.Include(g => g.DeceasedPerson).FirstOrDefaultAsync(g => g.GraveId == id);
            if (grave == null)
                return NotFound();

            return View(grave);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var grave = await _db.Graves.FindAsync(id);
            if (grave == null)
                return NotFound();

            return View(grave);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grave = await _db.Graves.FindAsync(id);
            if (grave == null)
                return NotFound();

            _db.Graves.Remove(grave);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
