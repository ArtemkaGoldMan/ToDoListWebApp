@AddingAndViewingTasks
Feature: Adding and Viewing Tasks
  As a user
  I want to add and view tasks
  So that I can manage my to-do list effectively

  Scenario: Adding a new task
    Given I have an empty to-do list
    When I add a task "Buy groceries" with priority "High" and deadline "2025-01-15"
    Then the task list should contain 1 task
    And the task "Buy groceries" should have priority "High"
    And the task "Buy groceries" should have a deadline "2025-01-15"

  Scenario: Adding a duplicate task
    Given I have an empty to-do list
    When I add a task "Buy groceries" with priority "High" and deadline "2025-01-15"
    And I attempt to add a duplicate task "Buy groceries" with priority "Medium" and deadline "2025-01-20"
    Then I should see an error "Task already exists."


  Scenario: Viewing all tasks
    Given I have an empty to-do list
    When I add a task "Buy groceries" with priority "High" and deadline "2025-01-15"
    And I add a task "Clean the house" with priority "Medium" and deadline "2025-01-20"
    Then the task list should contain 2 tasks
