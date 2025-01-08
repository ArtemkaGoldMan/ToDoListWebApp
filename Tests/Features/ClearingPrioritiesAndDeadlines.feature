@ClearingPrioritiesAndDeadlines
Feature: Clearing Priorities and Deadlines
  As a user
  I want to clear priorities or deadlines from tasks
  So that I can leave them unspecified when needed

  Scenario: Clearing a task's priority
    Given I have a task "Buy groceries" with priority "High"
    When I clear the priority of the task
    Then the task "Buy groceries" should have priority "None"

  Scenario: Clearing a task's deadline
    Given I have a task "Buy groceries" with a deadline "2025-01-15"
    When I clear the deadline of the task
    Then the task "Buy groceries" should have no deadline
