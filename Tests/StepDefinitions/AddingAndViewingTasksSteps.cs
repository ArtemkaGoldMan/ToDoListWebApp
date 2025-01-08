using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using ToDoWEB.Data;
using ToDoWEB.Models;
using ToDoWEB.Services;

namespace Tests.StepDefinitions
{
    [Binding]
    [Scope(Tag = "AddingAndViewingTasks")]
    public class AddingAndViewingTasksSteps : IDisposable
    {
        private readonly ToDoContext _context;
        private readonly ITaskService _taskService;
        private string _errorMessage;

        public AddingAndViewingTasksSteps()
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

        [When(@"I add a task ""(.*)"" with priority ""(.*)"" and deadline ""(.*)""")]
        public async Task WhenIAddATaskWithPriorityAndDeadline(string description, string priority, DateTime deadline)
        {
            await _taskService.AddTaskAsync(description, Enum.Parse<PriorityLevel>(priority), deadline);
        }

        [When(@"I attempt to add a duplicate task ""(.*)"" with priority ""(.*)"" and deadline ""(.*)""")]
        public async Task WhenIAttemptToAddADuplicateTask(string description, string priority, DateTime deadline)
        {
            try
            {
                await _taskService.AddTaskAsync(description, Enum.Parse<PriorityLevel>(priority), deadline);
            }
            catch (InvalidOperationException ex)
            {
                _errorMessage = ex.Message;
            }
        }

        [Then(@"I should see an error ""(.*)""")]
        public void ThenIShouldSeeAnError(string expectedError)
        {
            _errorMessage.Should().NotBeNull();
            _errorMessage.Should().Be(expectedError);
        }

        [Then(@"the task list should contain (.*) task")]
        public void ThenTheTaskListShouldContainTask(int count)
        {
            _context.Tasks.Count().Should().Be(count);
        }

        [Then(@"the task ""(.*)"" should have priority ""(.*)""")]
        public void ThenTheTaskShouldHavePriority(string description, string priority)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
            task.Should().NotBeNull();
            task.Priority.Should().Be(Enum.Parse<PriorityLevel>(priority));
        }

        [Then(@"the task ""(.*)"" should have a deadline ""(.*)""")]
        public void ThenTheTaskShouldHaveADeadline(string description, DateTime deadline)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
            task.Should().NotBeNull();
            task.Deadline.Should().Be(deadline);
        }

        [Then(@"the task list should contain (.*) tasks")]
        public void ThenTheTaskListShouldContainTasks(int count)
        {
            _context.Tasks.Count().Should().Be(count);
        }

        public void Dispose()
        {
            _context.Database.RollbackTransaction();
            _context.Dispose();
        }
    }
}
