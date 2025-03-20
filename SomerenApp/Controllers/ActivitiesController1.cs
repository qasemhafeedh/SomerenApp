using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SomerenApp.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly SomerenDbContext _context;

        public ActivitiesController(SomerenDbContext context)
        {
            _context = context;
        }

        // ✅ Improved: Index method with filtering, sorting, and exception handling
        public async Task<IActionResult> Index(string nameFilter)
        {
            try
            {
                var activities = _context.Activities.AsQueryable();

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    activities = activities.Where(a => a.ActivityName.Contains(nameFilter));
                }

                return View(await activities.OrderBy(a => a.ActivityDateTime).ToListAsync());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving activities. Please try again.";
                Console.WriteLine($"Error fetching activities: {ex.Message}");
                return View(new List<Activity>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        // ✅ Improved: Exception handling in Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error saving activity. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Create", activity);
                }
            }
            return View("Create", activity);
        }

        // ✅ Improved: Using reusable method to get activity by ID
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound();

            return View(activity);
        }

        // ✅ Improved: Exception handling in Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Activities.Update(activity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error updating activity. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Edit", activity);
                }
            }
            return View("Edit", activity);
        }

        // ✅ Improved: Using reusable method to get activity by ID
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await GetActivityByIdAsync(id);
            if (activity == null)
                return NotFound();

            return View("Delete", activity);
        }

        // ✅ Improved: Delete confirmation with exception handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ActivityID)
        {
            var activity = await GetActivityByIdAsync(ActivityID);
            if (activity == null)
                return NotFound();

            try
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
                Console.WriteLine("Activity deleted successfully.");
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorMessage = "Cannot delete activity. This activity is assigned to students or lecturers.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Improved: Reusable method to fetch activity by ID
        private async Task<Activity> GetActivityByIdAsync(int id)
        {
            return await _context.Activities.FirstOrDefaultAsync(a => a.ActivityID == id);
        }
    }
}
