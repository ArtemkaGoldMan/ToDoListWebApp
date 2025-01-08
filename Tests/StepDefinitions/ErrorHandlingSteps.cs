using FluentAssertions;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions;

[Binding]
[Scope(Tag = "ErrorHandling")]
public class ErrorHandlingSteps : IDisposable
{
    private readonly ToDoContext _context;
    private readonly ITaskService _taskService;
    private string _errorMessage;

    public ErrorHandlingSteps()
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

    [When(@"I add a task ""(.*)"" with an invalid priority ""(.*)""")]
    public async Task WhenIAddATaskWithAnInvalidPriority(string description, string invalidPriority)
    {
        try
        {
            // Attempt to parse the priority
            if (!Enum.TryParse<PriorityLevel>(invalidPriority, true, out var priority))
            {
                throw new ArgumentException("Invalid priority level");
            }

            await _taskService.AddTaskAsync(description, priority, null);
        }
        catch (ArgumentException ex)
        {
            _errorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
        }
    }

    [When(@"I edit a task with ID ""(.*)"" to have a new description ""(.*)""")]
    public async Task WhenIEditATaskWithIdToHaveANewDescription(int id, string newDescription)
    {
        try
        {
            await _taskService.EditTaskAsync(id, newDescription, null, null);
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
