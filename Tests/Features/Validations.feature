@TaskValidations
Feature: Task Validations
  As a user
  I want the system to validate task inputs
  So that I can avoid errors

  Scenario: Adding a task without a description
    Given I have an empty to-do list
    When I add a task with an empty description
    Then I should see an error message "Task description cannot be empty."

  Scenario: Preventing duplicate tasks
    Given I have a task "Buy groceries" in the to-do list
    When I add a task "Buy groceries" with priority "High" and deadline "2025-01-15"
    Then I should see an error message "Task already exists."
