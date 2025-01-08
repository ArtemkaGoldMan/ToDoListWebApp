Feature: Filtering and Sorting Tasks
  As a user
  I want to filter and sort tasks
  So that I can organize my to-do list

  Scenario: Viewing overdue tasks
    Given I have tasks "Buy groceries" (deadline: 2023-12-01) and "Clean the house" (deadline: 2025-01-15) in the to-do list
    When I view overdue tasks
    Then I should see only the task "Buy groceries"

  Scenario: Viewing tasks sorted by priority
    Given I have tasks "Buy groceries" (priority: High) and "Clean the house" (priority: Low) in the to-do list
    When I view tasks sorted by priority
    Then the first task should be "Buy groceries"

  Scenario: Viewing tasks sorted by deadline
    Given I have tasks "Buy groceries" (deadline: 2025-01-20) and "Clean the house" (deadline: 2025-01-15) in the to-do list
    When I view tasks sorted by deadline
    Then the first task should be "Clean the house"
