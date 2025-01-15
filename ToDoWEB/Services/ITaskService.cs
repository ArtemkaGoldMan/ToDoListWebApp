using ToDoWEB.Models;

namespace ToDoWEB.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasky>> GetAllTasksAsync();
        Task<Tasky> GetTaskByIdAsync(int id);
        Task AddTaskAsync(string description, PriorityLevel priority, DateTime? deadline = null);
        Task EditTaskAsync(int id, string newDescription, PriorityLevel? newPriority = null, DateTime? newDeadline = null);
        Task CompleteTaskAsync(int id);
        Task DeleteTaskAsync(int id);
        Task UncompleteTaskAsync(int id);
    }
}
