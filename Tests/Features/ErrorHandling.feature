@ErrorHandling
Feature: Error Handling
  As a user
  I want the system to handle errors gracefully
  So that I can understand what went wrong

  Scenario: Adding a task with an invalid priority
    Given I have an empty to-do list
    When I add a task "Buy groceries" with an invalid priority "Urgent"
    Then I should see an error message "Invalid priority level"

  Scenario: Editing a non-existent task
    Given I have an empty to-do list
    When I edit a task with ID "9999" to have a new description "Buy groceries"
    Then I should see an error message "Task not found."
