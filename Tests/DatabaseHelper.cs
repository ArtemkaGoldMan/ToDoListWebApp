using Microsoft.EntityFrameworkCore;
using ToDoWEB.Data;

namespace Tests
{
    public static class DatabaseHelper
    {
        public static ToDoContext GetSqlServerDbContext()
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ToDoTestDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var context = new ToDoContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
