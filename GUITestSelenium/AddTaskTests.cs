using OpenQA.Selenium;
using Xunit;

namespace GUITestSelenium
{
    public class AddTaskTests : BaseTest
    {
        [Fact]
        public void NavigateToAddTaskPage_ShouldDisplayAddTaskForm()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");
            Assert.Contains("Add Task", Driver.PageSource);
        }

        [Fact]
        public void CreateTask_WithValidInputs_ShouldDisplayInTaskList()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");

            Driver.FindElement(By.Id("Description")).SendKeys("Test Task");
            Driver.FindElement(By.Id("Priority")).SendKeys("None");
            Driver.FindElement(By.Id("Deadline")).SendKeys("2025-12-31");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Task");

            var taskRow = Driver.FindElement(By.XPath("//td[text()='Test Task']"));
            Assert.NotNull(taskRow);
        }


        [Fact]
        public void CreateTask_WithoutDescription_ShouldShowValidationError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");

            Driver.FindElement(By.Id("Priority")).SendKeys("Low");
            Driver.FindElement(By.Id("Deadline")).SendKeys("2025-12-31");

            var saveButton = Driver.FindElement(By.CssSelector("button[type='submit']"));
            saveButton.Click();

            Assert.Equal($"{BaseUrl}/Task/Create", Driver.Url);

            var descriptionField = Driver.FindElement(By.Id("Description"));
            Assert.True(descriptionField.GetAttribute("validationMessage").Contains("Please fill out this field."));
        }


        [Fact]
        public void CreateTask_WithDuplicateDescription_ShouldShowError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");
            Driver.FindElement(By.Id("Description")).SendKeys("Duplicate Task");
            Driver.FindElement(By.Id("Priority")).SendKeys("Medium");
            Driver.FindElement(By.Id("Deadline")).SendKeys("2025-12-31");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");
            Driver.FindElement(By.Id("Description")).SendKeys("Duplicate Task");
            Driver.FindElement(By.Id("Priority")).SendKeys("Medium");
            Driver.FindElement(By.Id("Deadline")).SendKeys("2025-12-31");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert an error is displayed
            Assert.Contains("alert alert-danger", Driver.PageSource);

            // Optionally, assert for a generic error message
            //Assert.Contains("Task with the same description already exists.", Driver.PageSource);
        }

        [Fact]
        public void CreateTask_WithPastDeadline_ShouldShowError()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");
            Driver.FindElement(By.Id("Description")).SendKeys("Task With Past Deadline");
            Driver.FindElement(By.Id("Priority")).SendKeys("High");
            Driver.FindElement(By.Id("Deadline")).SendKeys("2020-01-01");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert an error is displayed
            Assert.Contains("alert alert-danger", Driver.PageSource);

            // Optionally, assert for a generic error message
            Assert.Contains("An unexpected error occurred:", Driver.PageSource);
        }

        [Fact]
        public void CreateTask_WithEmptyDeadline_ShouldSucceed()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Task/Create");
            Driver.FindElement(By.Id("Description")).SendKeys("No Deadline Task");
            Driver.FindElement(By.Id("Priority")).SendKeys("High");
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Task");
            Assert.Contains("No Deadline Task", Driver.PageSource);
        }
    }
}
