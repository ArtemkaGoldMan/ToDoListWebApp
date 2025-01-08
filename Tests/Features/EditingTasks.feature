@EditingTasks
Feature: Editing Tasks
  As a user
  I want to edit tasks
  So that I can update task details


  Scenario: Editing a task description
    Given I have a task "Buy groceries" in the to-do list
    When I edit the task "Buy groceries" to "Buy groceries and milk"
    Then the task "Buy groceries and milk" should exist in the to-do list

  Scenario: Editing a task priority
    Given I have a task "Buy groceries" with priority "Medium"
    When I change the priority of the task to "High"
    Then the task "Buy groceries" should have priority "High"

  Scenario: Editing a task deadline
    Given I have a task "Buy groceries" with a deadline "2025-01-15"
    When I change the deadline of the task to "2025-01-20"
    Then the task "Buy groceries" should have a deadline "2025-01-20"
