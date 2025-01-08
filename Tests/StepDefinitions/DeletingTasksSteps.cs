using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "DeletingTasks")]
public class DeletingTasksSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;
    private string _errorMessage;

    public DeletingTasksSteps()
    {
        _context = DatabaseHelper.GetSqlServerDbContext();
        _taskService = new TaskService(_context);
        _context.Database.BeginTransaction();
    }

    [Given(@"I have a task ""(.*)"" in the to-do list")]
    public async Task GivenIHaveATaskInTheToDoList(string description)
    {
        await _taskService.AddTaskAsync(description, PriorityLevel.Medium, null);
    }

    [Given(@"I have an empty to-do list")]
    public void GivenIHaveAnEmptyToDoList()
    {
        _context.Tasks.RemoveRange(_context.Tasks);
        _context.SaveChanges();
    }

    [When(@"I delete the task ""(.*)""")]
    public async Task WhenIDeleteTheTask(string description)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        if (task != null)
        {
            await _taskService.DeleteTaskAsync(task.Id);
        }
        else
        {
            // Task does not exist, set the error message
            _errorMessage = "Task not found";
        }
    }

    [Then(@"the task ""(.*)"" should no longer exist in the to-do list")]
    public void ThenTheTaskShouldNoLongerExistInTheToDoList(string description)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
        task.Should().BeNull();
    }

    [Then(@"I should see an error message ""(.*)""")]
    public void ThenIShouldSeeAnErrorMessage(string expectedMessage)
    {
        _errorMessage.Should().NotBeNull();
        _errorMessage.Should().Be(expectedMessage);
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }
}
