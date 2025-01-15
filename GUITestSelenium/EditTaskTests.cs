using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUITestSelenium
{
    public class EditTaskTests : BaseTest
    {

        int TestTaskId = 1;

        [Fact]
        public void NavigateToAddTaskPage_ShouldDisplayAddTaskForm()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit");
            Assert.Contains("Add Task", Driver.PageSource);
        }

        [Fact]
        public void EditTask_WithValidInputs_ShouldUpdateTask()
        {
            // Navigate to the Edit page for the specific task
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Update the task description, priority, and deadline
            var descriptionField = Driver.FindElement(By.Id("Description"));
            descriptionField.Clear();
            descriptionField.SendKeys("Updated Task Description");

            var priorityField = Driver.FindElement(By.Id("Priority"));
            priorityField.SendKeys("High");

            var deadlineField = Driver.FindElement(By.Id("Deadline"));
            deadlineField.Clear();
            deadlineField.SendKeys("2025-12-31");

            // Submit the form
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Verify that the changes are reflected in the task list
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task");
            var updatedTaskRow = Driver.FindElement(By.XPath("//tr[td[text()='Updated Task Description']]"));
            Assert.NotNull(updatedTaskRow);

            var updatedPriority = updatedTaskRow.FindElement(By.XPath("td[2]")); // Priority column
            Assert.Equal("High", updatedPriority.Text.Trim());
        }

        [Fact]
        public void EditTask_WithDuplicateDescription_ShouldShowError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Attempt to set a duplicate description
            var descriptionField = Driver.FindElement(By.Id("Description"));
            descriptionField.Clear();
            descriptionField.SendKeys("Duplicate Task Description"); // Ensure this description already exists

            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert that an error message is displayed
            Assert.Contains("Another task with the same description already exists", Driver.PageSource);
        }

        [Fact]
        public void EditTask_WithPastDeadline_ShouldShowError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Attempt to set a past deadline
            var deadlineField = Driver.FindElement(By.Id("Deadline"));
            deadlineField.Clear();
            deadlineField.SendKeys("2020-01-01");

            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert that an error message is displayed
            Assert.Contains("Deadline cannot be in the past", Driver.PageSource);
        }

        [Fact]
        public void EditTask_WithEmptyDescription_ShouldShowError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Attempt to set an empty description
            var descriptionField = Driver.FindElement(By.Id("Description"));
            descriptionField.Clear();

            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert that the form remains on the Edit page and an error is displayed
            Assert.Contains("Please fill out this field.", Driver.PageSource);
        }

        [Fact]
        public void CancelEditTask_ShouldReturnToTaskList()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Navigate back to the task list without saving changes
            Driver.FindElement(By.CssSelector("a[href='/Task']")).Click();

            // Assert that the task list page is displayed
            Assert.Contains("To-Do List", Driver.PageSource);
        }

        [Fact]
        public void EditTask_WithoutChangingAnything_ShouldRetainOriginalValues()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Edit/{TestTaskId}");

            // Simply submit the form without making any changes
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Verify the original values are retained in the task list
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task");
            var taskRow = Driver.FindElement(By.XPath("//tr[td[text()='Original Task Description']]")); // Replace with the actual original description
            Assert.NotNull(taskRow);
        }
    }
}
