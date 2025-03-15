using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SomerenApp.Controllers
{
    public class LecturersController : Controller
    {
        private readonly SomerenDbContext _context;

        public LecturersController(SomerenDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lecturers = await _context.Lecturers.ToListAsync();
            return View(lecturers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
                return NotFound();

            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Update(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
                return NotFound();

            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            try
            {
                _context.Lecturers.Remove(lecturer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete. The lecturer might be linked to other records.");
                return View("Delete", lecturer);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}