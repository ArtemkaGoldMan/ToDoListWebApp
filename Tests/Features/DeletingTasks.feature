Feature: Deleting Tasks
  As a user
  I want to delete tasks
  So that I can remove unnecessary tasks

  Scenario: Deleting a task
    Given I have a task "Buy groceries" in the to-do list
    When I delete the task "Buy groceries"
    Then the task "Buy groceries" should no longer exist in the to-do list

  Scenario: Deleting a task that doesn’t exist
    Given I have an empty to-do list
    When I delete the task "Buy groceries"
    Then I should see an error message "Task not found"
