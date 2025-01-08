using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoWEB.Data;
using ToDoWEB.Models;

namespace ToDoWEB.Services
{
    public class TaskService : ITaskService
    {
        private readonly ToDoContext _context;

        public TaskService(ToDoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasky>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Tasky> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task AddTaskAsync(string description, PriorityLevel priority, DateTime? deadline = null)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Task description cannot be empty.");

            if (await _context.Tasks.AnyAsync(t => t.Description == description))
                throw new InvalidOperationException("Task already exists.");

            var task = new Tasky
            {
                Description = description,
                Priority = priority,
                CreatedAt = DateTime.Now,
                Deadline = deadline
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task EditTaskAsync(int id, string newDescription, PriorityLevel? newPriority = null, DateTime? newDeadline = null)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            if (!string.IsNullOrWhiteSpace(newDescription))
                task.Description = newDescription;

            if (newPriority.HasValue)
                task.Priority = newPriority.Value;

            if (newDeadline.HasValue)
                task.Deadline = newDeadline;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task CompleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
