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
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return View(tasks);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while loading tasks: " + ex.Message);
                return View("Error");
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string description, PriorityLevel priority, DateTime? deadline)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _taskService.AddTaskAsync(description, priority, deadline);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null) return NotFound();
                return View(task);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while loading the task: " + ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string description, PriorityLevel priority, DateTime? deadline)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _taskService.EditTaskAsync(id, description, priority, deadline);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null) return NotFound();
                return View(task);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while loading the task: " + ex.Message);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the task: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                await _taskService.CompleteTaskAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Uncomplete(int id)
        {
            try
            {
                await _taskService.UncompleteTaskAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred: " + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}