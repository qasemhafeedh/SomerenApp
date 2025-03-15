
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SomerenApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SomerenDbContext _context;

        public StudentsController(SomerenDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View("Index", students);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", student);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            return View("Edit", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", student);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            return View("Delete", student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}