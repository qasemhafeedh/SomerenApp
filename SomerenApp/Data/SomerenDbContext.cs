using Microsoft.EntityFrameworkCore;
using SomerenApp.Models;

namespace SomerenApp.Data
{
    public class SomerenDbContext : DbContext
    {
        public SomerenDbContext(DbContextOptions<SomerenDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}

