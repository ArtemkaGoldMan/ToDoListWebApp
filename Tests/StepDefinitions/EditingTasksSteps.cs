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
    [Scope(Tag = "EditingTasks")]
    public class EditingTasksSteps : IDisposable
    {
        private readonly ToDoContext _context;
        private readonly ITaskService _taskService;

        public EditingTasksSteps()
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

        [Given(@"I have a task ""(.*)"" with priority ""(.*)""")]
        public async Task GivenIHaveATaskWithPriority(string description, string priority)
        {
            await _taskService.AddTaskAsync(description, Enum.Parse<PriorityLevel>(priority), null);
        }

        [Given(@"I have a task ""(.*)"" with a deadline ""(.*)""")]
        public async Task GivenIHaveATaskWithADeadline(string description, DateTime deadline)
        {
            await _taskService.AddTaskAsync(description, PriorityLevel.Medium, deadline);
        }

        [When(@"I edit the task ""(.*)"" to ""(.*)""")]
        public async Task WhenIEditTheTaskTo(string oldDescription, string newDescription)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Description == oldDescription);
            if (task != null)
            {
                await _taskService.EditTaskAsync(task.Id, newDescription, task.Priority, task.Deadline);
            }
        }

        [When(@"I change the priority of the task to ""(.*)""")]
        public async Task WhenIChangeThePriorityOfTheTaskTo(string newPriority)
        {
            var task = _context.Tasks.First();
            if (task != null)
            {
                await _taskService.EditTaskAsync(task.Id, task.Description, Enum.Parse<PriorityLevel>(newPriority), task.Deadline);
            }
        }

        [When(@"I change the deadline of the task to ""(.*)""")]
        public async Task WhenIChangeTheDeadlineOfTheTaskTo(DateTime newDeadline)
        {
            var task = _context.Tasks.First();
            if (task != null)
            {
                await _taskService.EditTaskAsync(task.Id, task.Description, task.Priority, newDeadline);
            }
        }

        [Then(@"the task ""(.*)"" should exist in the to-do list")]
        public void ThenTheTaskShouldExistInTheToDoList(string description)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Description == description);
            task.Should().NotBeNull();
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

        public void Dispose()
        {
            _context.Database.RollbackTransaction();
            _context.Dispose();
        }
    }
}
