using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "ClearingPrioritiesAndDeadlines")]
public class ClearingPrioritiesAndDeadlinesSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;

    public ClearingPrioritiesAndDeadlinesSteps()
    {
        _context = DatabaseHelper.GetSqlServerDbContext();
        _taskService = new TaskService(_context);
        _context.Database.BeginTransaction();
    }

    [Given(@"I have a task ""(.*)"" with priority ""(.*)""")]
    public async Task GivenIHaveATaskWithPriority(string description, string priority)
    {
        await _taskService.AddTaskAsync(description, Enum.Parse<PriorityLevel>(priority), null);
    }

    [Given(@"I have a task ""(.*)"" with a deadline ""(.*)""")]
    public async Task GivenIHaveATaskWithADeadline(string description, string deadline)
    {
        await _taskService.AddTaskAsync(description, PriorityLevel.Medium, DateTime.Parse(deadline));
    }

    [When(@"I clear the priority of the task")]
    public async Task WhenIClearThePriorityOfTheTask()
    {
        var task = _context.Tasks.FirstOrDefault();
        if (task != null)
        {
            await _taskService.EditTaskAsync(task.Id, task.Description, PriorityLevel.None, task.Deadline);
        }
    }

    [When(@"I clear the deadline of the task")]
    public async Task WhenIClearTheDeadlineOfTheTask()
    {
        var task = _context.Tasks.FirstOrDefault();
        if (task != null)
        {
            await _taskService.EditTaskAsync(task.Id, task.Description, task.Priority, null);
        }
    }

    [Then(@"the task ""(.*)"" should have priority ""(.*)""")]
    public void ThenTheTaskShouldHavePriority(string description, string priority)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        task.Should().NotBeNull();
        task.Priority.Should().Be(Enum.Parse<PriorityLevel>(priority));
    }

    [Then(@"the task ""(.*)"" should have no deadline")]
    public void ThenTheTaskShouldHaveNoDeadline(string description)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        task.Should().NotBeNull();
        task.Deadline.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }
}
