using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "CompletingTasks")]
public class CompletingTasksSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;
    private IEnumerable<Tasky> _viewedTasks;

    public CompletingTasksSteps()
    {
        _context = DatabaseHelper.GetSqlServerDbContext();
        _taskService = new TaskService(_context);
        _context.Database.BeginTransaction();
    }

    // Scenario: Marking a task as completed
    [Given(@"I have a task ""(.*)"" in the to-do list")]
    public async Task GivenIHaveATaskInTheToDoList(string description)
    {
        await _taskService.AddTaskAsync(description, PriorityLevel.Medium, null);
    }

    [When(@"I mark the task ""(.*)"" as completed")]
    public async Task WhenIMarkTheTaskAsCompleted(string description)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        if (task != null)
        {
            await _taskService.CompleteTaskAsync(task.Id);
        }
    }

    [Then(@"the task ""(.*)"" should be marked as completed")]
    public void ThenTheTaskShouldBeMarkedAsCompleted(string description)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        task.Should().NotBeNull();
        task.IsCompleted.Should().BeTrue();
    }

    // Scenario: Viewing only completed tasks
    [Given(@"I have tasks ""(.*)"" \(completed\) and ""(.*)"" \(active\) in the to-do list")]
    public async Task GivenIHaveTasksCompletedAndActiveInTheToDoList(string completedTaskDescription, string activeTaskDescription)
    {
        // Add completed task
        await _taskService.AddTaskAsync(completedTaskDescription, PriorityLevel.Medium, null);
        var completedTask = _context.Tasks.FirstOrDefault(t => t.Description == completedTaskDescription);
        if (completedTask != null)
        {
            await _taskService.CompleteTaskAsync(completedTask.Id);
        }

        // Add active task
        await _taskService.AddTaskAsync(activeTaskDescription, PriorityLevel.Medium, null);
    }

    [When(@"I view completed tasks")]
    public async Task WhenIViewCompletedTasks()
    {
        _viewedTasks = await _taskService.GetAllTasksAsync();
        _viewedTasks = _viewedTasks.Where(t => t.IsCompleted);
    }

    [Then(@"I should see only the task ""(.*)""")]
    public void ThenIShouldSeeOnlyTheTask(string expectedDescription)
    {
        _viewedTasks.Should().ContainSingle(t => t.Description == expectedDescription);
    }

    // Scenario: Viewing only active tasks
    [When(@"I view active tasks")]
    public async Task WhenIViewActiveTasks()
    {
        _viewedTasks = await _taskService.GetAllTasksAsync();
        _viewedTasks = _viewedTasks.Where(t => !t.IsCompleted);
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }
}
