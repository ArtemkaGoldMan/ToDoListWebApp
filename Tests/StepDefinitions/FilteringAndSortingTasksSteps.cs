using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "FilteringAndSortingTasks")]
public class FilteringAndSortingTasksSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;
    private IEnumerable<Tasky> _viewedTasks;

    public FilteringAndSortingTasksSteps()
    {
        _context = DatabaseHelper.GetSqlServerDbContext();
        _taskService = new TaskService(_context);
        _context.Database.BeginTransaction();
    }

    // Scenario: Viewing overdue tasks
    [Given(@"I have tasks ""(.*)"" \(deadline: (.*)\) and ""(.*)"" \(deadline: (.*)\) in the to-do list")]
    public async Task GivenIHaveTasksWithDeadlinesInTheToDoList(string task1Description, string task1Deadline, string task2Description, string task2Deadline)
    {
        var deadline1 = DateTime.Parse(task1Deadline);
        var deadline2 = DateTime.Parse(task2Deadline);

        await _taskService.AddTaskAsync(task1Description, PriorityLevel.Medium, deadline1);
        await _taskService.AddTaskAsync(task2Description, PriorityLevel.Medium, deadline2);
    }

    [When(@"I view overdue tasks")]
    public async Task WhenIViewOverdueTasks()
    {
        var allTasks = await _taskService.GetAllTasksAsync();
        var today = DateTime.Today;
        _viewedTasks = allTasks.Where(t => t.Deadline.HasValue && t.Deadline.Value.Date < today && !t.IsCompleted);
    }

    [Then(@"I should see only the task ""(.*)""")]
    public void ThenIShouldSeeOnlyTheTask(string expectedTaskDescription)
    {
        _viewedTasks.Should().ContainSingle(t => t.Description == expectedTaskDescription);
    }

    // Scenario: Viewing tasks sorted by priority
    [Given(@"I have tasks ""(.*)"" \(priority: (.*)\) and ""(.*)"" \(priority: (.*)\) in the to-do list")]
    public async Task GivenIHaveTasksWithPrioritiesInTheToDoList(string task1Description, string task1Priority, string task2Description, string task2Priority)
    {
        var priority1 = Enum.Parse<PriorityLevel>(task1Priority);
        var priority2 = Enum.Parse<PriorityLevel>(task2Priority);

        await _taskService.AddTaskAsync(task1Description, priority1, null);
        await _taskService.AddTaskAsync(task2Description, priority2, null);
    }

    [When(@"I view tasks sorted by priority")]
    public async Task WhenIViewTasksSortedByPriority()
    {
        var allTasks = await _taskService.GetAllTasksAsync();
        _viewedTasks = allTasks.OrderByDescending(t => t.Priority);
    }

    [Then(@"the first task should be ""(.*)""")]
    public void ThenTheFirstTaskShouldBe(string expectedTaskDescription)
    {
        _viewedTasks.First().Description.Should().Be(expectedTaskDescription);
    }

    // Scenario: Viewing tasks sorted by deadline
    // Reuse the Given step for tasks with deadlines
    [When(@"I view tasks sorted by deadline")]
    public async Task WhenIViewTasksSortedByDeadline()
    {
        var allTasks = await _taskService.GetAllTasksAsync();
        _viewedTasks = allTasks.Where(t => t.Deadline.HasValue).OrderBy(t => t.Deadline.Value);
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }
}
