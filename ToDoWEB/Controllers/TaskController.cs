using Microsoft.AspNetCore.Mvc;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace ToDoWEB.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string description, PriorityLevel priority, DateTime? deadline)
        {
            if (ModelState.IsValid)
            {
                await _taskService.AddTaskAsync(description, priority, deadline);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string description, PriorityLevel priority, DateTime? deadline)
        {
            if (ModelState.IsValid)
            {
                await _taskService.EditTaskAsync(id, description, priority, deadline);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Complete(int id)
        {
            await _taskService.CompleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
