using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SomerenApp.Data;
using SomerenApp.Models;
using System;
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

        // ✅ Improved: Index method with filtering, sorting, and exception handling
        public async Task<IActionResult> Index(string lastNameFilter)
        {
            try
            {
                var students = _context.Students.AsQueryable();

                if (!string.IsNullOrEmpty(lastNameFilter))
                {
                    students = students.Where(s => s.LastName.Contains(lastNameFilter));
                }

                return View(await students.OrderBy(s => s.LastName).ToListAsync());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving students. Please try again.";
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return View(new List<Student>());
            }
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        // ✅ Improved: Exception handling in Create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error saving student. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Create", student);
                }
            }
            return View("Create", student);
        }

        // ✅ Improved: Using reusable method to get student by ID
        public async Task<IActionResult> Edit(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return View("Edit", student);
        }

        // ✅ Improved: Exception handling in Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ViewBag.ErrorMessage = "Error updating student. Please try again.";
                    Console.WriteLine($"Database error: {ex.Message}");
                    return View("Edit", student);
                }
            }
            return View("Edit", student);
        }

        // ✅ Improved: Using reusable method to get student by ID
        public async Task<IActionResult> Delete(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return View("Delete", student);
        }

        // ✅ Improved: Delete confirmation with exception handling
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int StudentID)
        {
            var student = await GetStudentByIdAsync(StudentID);
            if (student == null)
                return NotFound();

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                Console.WriteLine("Student deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                ViewBag.ErrorMessage = "Cannot delete student. This student may be linked to other data.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Improved: Reusable method to fetch student by ID
        private async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
        }
    }
}
