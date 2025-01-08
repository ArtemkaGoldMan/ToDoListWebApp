namespace ToDoWEB.Models
{
    public class Tasky
    {
        public int Id { get; set; } 
        public string Description { get; set; } // Tasky description
        public bool IsCompleted { get; set; } // Status of task (active/completed)
        public PriorityLevel Priority { get; set; } // Priority of the task
        public DateTime CreatedAt { get; set; } 
        public DateTime? Deadline { get; set; } 
        public DateTime? CompletedAt { get; set; }
    }
}
