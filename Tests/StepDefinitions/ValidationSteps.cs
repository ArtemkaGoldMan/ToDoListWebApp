using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "TaskValidations")]
public class ValidationsSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;
    private string _errorMessage;

    public ValidationsSteps()
    {
        _context = DatabaseHelper.GetSqlServerDbContext();
        _taskService = new TaskService(_context);
        _context.Database.BeginTransaction();
    }

    [Given(@"I have an empty to-do list")]
    public void GivenIHaveAnEmptyToDoList()
    {
        _context.Tasks.RemoveRange(_context.Tasks);
        _context.SaveChanges();
    }

    [Given(@"I have a task ""(.*)"" in the to-do list")]
    public async Task GivenIHaveATaskInTheToDoList(string description)
    {
        await _taskService.AddTaskAsync(description, PriorityLevel.Medium, null);
    }

    [When(@"I add a task with an empty description")]
    public async Task WhenIAddATaskWithAnEmptyDescription()
    {
        try
        {
            await _taskService.AddTaskAsync(string.Empty, PriorityLevel.Medium, null);
        }
        catch (ArgumentException ex)
        {
            _errorMessage = ex.Message;
        }
    }

    [When(@"I add a task ""(.*)"" with priority ""(.*)"" and deadline ""(.*)""")]
    public async Task WhenIAddATaskWithPriorityAndDeadline(string description, string priority, string deadline)
    {
        try
        {
            var parsedPriority = Enum.Parse<PriorityLevel>(priority);
            var parsedDeadline = DateTime.Parse(deadline);

            await _taskService.AddTaskAsync(description, parsedPriority, parsedDeadline);
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
        }
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
