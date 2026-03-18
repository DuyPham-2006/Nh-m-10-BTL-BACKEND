using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    public class DeceasedMvcController : Controller
    {
        private readonly AppDbContext _db;

        public DeceasedMvcController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _db.DeceasedPersons.Include(d => d.Grave).OrderBy(d => d.DeceasedId).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Graves = _db.Graves.Where(g => g.Status == "Available").OrderBy(g => g.GraveId).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeceasedPerson model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Graves = _db.Graves.Where(g => g.Status == "Available").OrderBy(g => g.GraveId).ToList();
                return View(model);
            }

            model.CreatedDate = DateTime.UtcNow;
            _db.DeceasedPersons.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.DeceasedPersons.FindAsync(id);
            if (item == null)
                return NotFound();

            ViewBag.Graves = _db.Graves.Where(g => g.Status == "Available").OrderBy(g => g.GraveId).ToList();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeceasedPerson model)
        {
            if (id != model.DeceasedId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Graves = _db.Graves.Where(g => g.Status == "Available").OrderBy(g => g.GraveId).ToList();
                return View(model);
            }

            var existing = await _db.DeceasedPersons.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = model.Name;
            existing.BirthYear = model.BirthYear;
            existing.DeathYear = model.DeathYear;
            existing.Description = model.Description;
            existing.GraveId = model.GraveId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.DeceasedPersons
                .Include(d => d.Grave)
                .Include(d => d.Relatives)
                .FirstOrDefaultAsync(d => d.DeceasedId == id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.DeceasedPersons.Include(d => d.Grave).FirstOrDefaultAsync(d => d.DeceasedId == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.DeceasedPersons.FindAsync(id);
            if (item == null)
                return NotFound();

            _db.DeceasedPersons.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
