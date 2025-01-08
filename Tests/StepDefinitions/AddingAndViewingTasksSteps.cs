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
    public class AddingAndViewingTasksSteps : IDisposable
    {
        private readonly ToDoContext _context;
        private readonly ITaskService _taskService;

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

        [When(@"I add a task ""(.*)"" with priority ""(.*)"" and deadline ""(.*)"" again")]
        public async Task WhenIAddATaskWithPriorityAndDeadlineAgain(string description, string priority, DateTime deadline)
        {
            await _taskService.AddTaskAsync(description, Enum.Parse<PriorityLevel>(priority), deadline);
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
