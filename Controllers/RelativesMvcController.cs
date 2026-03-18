using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;
using PostManagementApp.Models;

namespace PostManagementApp.Controllers
{
    public class RelativesMvcController : Controller
    {
        private readonly AppDbContext _db;

        public RelativesMvcController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _db.Relatives.Include(r => r.DeceasedPerson).OrderBy(r => r.RelativeId).ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Deceased = _db.DeceasedPersons.OrderBy(d => d.DeceasedId).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Relative model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Deceased = _db.DeceasedPersons.OrderBy(d => d.DeceasedId).ToList();
                return View(model);
            }

            model.CreatedDate = DateTime.UtcNow;
            _db.Relatives.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Relatives.FindAsync(id);
            if (item == null)
                return NotFound();

            ViewBag.Deceased = _db.DeceasedPersons.OrderBy(d => d.DeceasedId).ToList();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Relative model)
        {
            if (id != model.RelativeId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Deceased = _db.DeceasedPersons.OrderBy(d => d.DeceasedId).ToList();
                return View(model);
            }

            var existing = await _db.Relatives.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.FullName = model.FullName;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Relationship = model.Relationship;
            existing.Email = model.Email;
            existing.Address = model.Address;
            existing.DeceasedId = model.DeceasedId;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.Relatives
                .Include(r => r.DeceasedPerson)
                .FirstOrDefaultAsync(r => r.RelativeId == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Relatives
                .Include(r => r.DeceasedPerson)
                .FirstOrDefaultAsync(r => r.RelativeId == id);
            if (item == null)
                return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Relatives.FindAsync(id);
            if (item == null)
                return NotFound();

            _db.Relatives.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
