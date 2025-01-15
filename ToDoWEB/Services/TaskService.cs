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
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving tasks.", ex);
            }
        }

        public async Task<Tasky> GetTaskByIdAsync(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return task;


        }

        public async Task AddTaskAsync(string description, PriorityLevel priority, DateTime? deadline = null)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Task description cannot be empty.");

            if (!Enum.IsDefined(typeof(PriorityLevel), priority))
                throw new ArgumentException("Invalid priority level.");

            if (deadline.HasValue && deadline.Value.Date < DateTime.Now.Date)
                throw new ArgumentException("Deadline cannot be in the past.");

            if (await _context.Tasks.AnyAsync(t => t.Description == description))
                throw new InvalidOperationException("Task with the same description already exists.");

            var task = new Tasky
            {
                Description = description,
                Priority = priority,
                CreatedAt = DateTime.Now,
                Deadline = deadline
            };

            try
            {
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the task.", ex);
            }
        }

        public async Task EditTaskAsync(int id, string newDescription, PriorityLevel? newPriority = null, DateTime? newDeadline = null)
        {

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                if (await _context.Tasks.AnyAsync(t => t.Description == newDescription && t.Id != id))
                    throw new InvalidOperationException("Another task with the same description already exists.");

                task.Description = newDescription;
            }

            if (newPriority.HasValue && !Enum.IsDefined(typeof(PriorityLevel), newPriority.Value))
                throw new ArgumentException("Invalid priority level.");

            if (newPriority.HasValue)
                task.Priority = newPriority.Value;

            if (newDeadline.HasValue && newDeadline < DateTime.Now)
                throw new ArgumentException("Deadline cannot be in the past.");

            if (newDeadline.HasValue)
                task.Deadline = newDeadline;


            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the task.", ex);
            }
        }

        public async Task CompleteTaskAsync(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            if (task.IsCompleted)
                throw new InvalidOperationException("Task is already completed.");

            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;

            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while completing the task.", ex);
            }
        }

        public async Task UncompleteTaskAsync(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            if (!task.IsCompleted)
                throw new InvalidOperationException("Task is not marked as completed.");

            task.IsCompleted = false;
            task.CompletedAt = null;

            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uncompleting the task.", ex);
            }
        }

        public async Task DeleteTaskAsync(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            try
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the task.", ex);
            }
        }
    }
}