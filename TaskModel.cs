using System;

namespace cybersecurity_chatbot_p2.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDueDate { get; set; }
        public string TaskStatus { get; set; }

        public TaskModel()
        {
            TaskStatus = "Pending";
        }

        public string GetDisplayString()
        {
            string statusSymbol = TaskStatus == "Completed" ? "[✓]" : "[○]";
            string dueDate = string.IsNullOrEmpty(TaskDueDate) ? "" : $" (Due: {TaskDueDate})";
            return $"{statusSymbol} {TaskName}{dueDate}";
        }

        public string GetFullDetails()
        {
            return $"Task: {TaskName}\n" +
                   $"Description: {TaskDescription ?? "N/A"}\n" +
                   $"Status: {TaskStatus}\n" +
                   $"Due Date: {TaskDueDate ?? "No due date"}";
        }
    }
}