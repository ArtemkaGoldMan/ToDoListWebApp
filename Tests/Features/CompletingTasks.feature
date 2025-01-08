Feature: Completing Tasks
  As a user
  I want to mark tasks as completed
  So that I can track finished work

  Scenario: Marking a task as completed
    Given I have a task "Buy groceries" in the to-do list
    When I mark the task "Buy groceries" as completed
    Then the task "Buy groceries" should be marked as completed

  Scenario: Viewing only completed tasks
    Given I have tasks "Buy groceries" (completed) and "Clean the house" (active) in the to-do list
    When I view completed tasks
    Then I should see only the task "Buy groceries"

  Scenario: Viewing only active tasks
    Given I have tasks "Buy groceries" (completed) and "Clean the house" (active) in the to-do list
    When I view active tasks
    Then I should see only the task "Clean the house"
