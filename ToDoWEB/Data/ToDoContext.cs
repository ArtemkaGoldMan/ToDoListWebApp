using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using ToDoWEB.Models;

namespace ToDoWEB.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<Tasky> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasky>()
                .Property(t => t.Priority)
                .HasConversion<string>(); // Store PriorityLevel as strings in the database
        }
    }
}
