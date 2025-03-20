using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
using System;
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

        // ✅ Improved: Index method with filtering, sorting, and exception handling
        public async Task<IActionResult> Index(string lastNameFilter)
        {
            try
            {
                var lecturers = _context.Lecturers.AsQueryable();

                if (!string.IsNullOrEmpty(lastNameFilter))
                {
                    lecturers = lecturers.Where(l => l.LastName.Contains(lastNameFilter));
                }

                return View(await lecturers.OrderBy(l => l.LastName).ToListAsync());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving lecturers. Please try again.";
                Console.WriteLine($"Error fetching lecturers: {ex.Message}");
                return View(new List<Lecturer>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        // ✅ Improved: Exception handling in Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Lecturers.Add(lecturer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error saving lecturer. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Create", lecturer);
                }
            }
            return View("Create", lecturer);
        }

        // ✅ Improved: Using reusable method to get lecturer by ID
        public async Task<IActionResult> Edit(int id)
        {
            var lecturer = await GetLecturerByIdAsync(id);
            if (lecturer == null)
                return NotFound();

            return View(lecturer);
        }

        // ✅ Improved: Exception handling in Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Lecturers.Update(lecturer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error updating lecturer. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Edit", lecturer);
                }
            }
            return View("Edit", lecturer);
        }

        // ✅ Improved: Using reusable method to get lecturer by ID
        public async Task<IActionResult> Delete(int id)
        {
            var lecturer = await GetLecturerByIdAsync(id);
            if (lecturer == null)
                return NotFound();

            return View("Delete", lecturer);
        }

        // ✅ Improved: Delete confirmation with exception handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int LecturerID)
        {
            var lecturer = await GetLecturerByIdAsync(LecturerID);
            if (lecturer == null)
                return NotFound();

            try
            {
                _context.Lecturers.Remove(lecturer);
                await _context.SaveChangesAsync();
                Console.WriteLine("Lecturer deleted successfully.");
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorMessage = "Cannot delete lecturer. This lecturer is assigned to activities.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Improved: Reusable method to fetch lecturer by ID
        private async Task<Lecturer> GetLecturerByIdAsync(int id)
        {
            return await _context.Lecturers.FirstOrDefaultAsync(l => l.LecturerID == id);
        }
    }
}
