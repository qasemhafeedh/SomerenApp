using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
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

        public async Task<IActionResult> Index()
        {
            var activities = await _context.Activities.ToListAsync();
            return View("Index", activities);
        }

        // Add Activity (GET)
        public IActionResult Create()
        {
            return View("Create");
        }

        // Add Activity (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Activities.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", activity);
        }

        // Edit Activity (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityID == id);
            if (activity == null)
                return NotFound();

            return View("Edit", activity);
        }

        // Edit Activity (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Activities.Update(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", activity);
        }

        // Delete Activity (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityID == id);
            if (activity == null)
                return NotFound();

            return View("Delete", activity);
        }

        // Delete Activity (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityID == id);
            if (activity == null)
                return NotFound();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}


