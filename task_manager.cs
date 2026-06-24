using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public class task_manager
    {
        private database_helper dbHelper;
        private List<TaskModel> currentTasks;
        private bool waitingForReminder;
        private string pendingTaskName;
        private string pendingTaskDescription;
        private Random random;

        public task_manager()
        {
            dbHelper = new database_helper();
            currentTasks = new List<TaskModel>();
            waitingForReminder = false;
            pendingTaskName = "";
            pendingTaskDescription = "";
            random = new Random();
            LoadTasks();
        }

        public List<TaskModel> GetTasks()
        {
            return currentTasks;
        }

        public void LoadTasks()
        {
            currentTasks = dbHelper.GetAllTasks();
        }

        public int GetPendingCount()
        {
            int count = 0;
            foreach (var task in currentTasks)
            {
                if (task.TaskStatus == "Pending")
                    count++;
            }
            return count;
        }

        public int GetCompletedCount()
        {
            int count = 0;
            foreach (var task in currentTasks)
            {
                if (task.TaskStatus == "Completed")
                    count++;
            }
            return count;
        }

        public string ProcessTaskInput(string input)
        {
            string lowerInput = input.ToLower().Trim();

            if (waitingForReminder)
            {
                return HandleReminderResponse(input);
            }

            if (lowerInput.StartsWith("add task") || lowerInput.Contains("add task") ||
                lowerInput.StartsWith("create task") || lowerInput.Contains("create task"))
            {
                return HandleAddTask(input);
            }

            if (lowerInput.Contains("show tasks") || lowerInput.Contains("view tasks") ||
                lowerInput.Contains("my tasks") || lowerInput.Contains("list tasks"))
            {
                return GetTaskList();
            }

            if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
            {
                return HandleDeleteTask(input);
            }

            if (lowerInput.Contains("complete task") || lowerInput.Contains("mark done") ||
                lowerInput.Contains("finish task"))
            {
                return HandleCompleteTask(input);
            }

            return null;
        }

        private string HandleAddTask(string input)
        {
            string taskName = input;
            taskName = Regex.Replace(taskName, @"^(add|create)\s+task\s*", "", RegexOptions.IgnoreCase);
            taskName = taskName.Trim();

            if (string.IsNullOrEmpty(taskName))
            {
                return "What task would you like to add? Please provide a task name.";
            }

            pendingTaskName = taskName;
            pendingTaskDescription = "Cybersecurity task";
            waitingForReminder = true;

            return $"Task added: '{taskName}'. Would you like to set a reminder? (yes/no)";
        }

        private string HandleReminderResponse(string input)
        {
            string lowerInput = input.ToLower().Trim();
            waitingForReminder = false;

            if (lowerInput.Contains("yes") || lowerInput.Contains("sure") || lowerInput.Contains("ok") || lowerInput.Contains("y"))
            {
                return "When would you like to be reminded? (e.g., 'in 3 days' or 'on 2026-06-30')";
            }
            else if (lowerInput.Contains("no") || lowerInput.Contains("nope") || lowerInput.Contains("n"))
            {
                return AddTaskToDatabase(pendingTaskName, pendingTaskDescription, null);
            }
            else
            {
                string reminderDate = ParseReminderDate(input);
                if (!string.IsNullOrEmpty(reminderDate))
                {
                    return AddTaskToDatabase(pendingTaskName, pendingTaskDescription, reminderDate);
                }
                else
                {
                    return "I didn't understand that. Task added without reminder.";
                }
            }
        }

        public string ProcessReminderDate(string input)
        {
            string reminderDate = ParseReminderDate(input);
            if (!string.IsNullOrEmpty(reminderDate))
            {
                return AddTaskToDatabase(pendingTaskName, pendingTaskDescription, reminderDate);
            }
            else
            {
                return "I didn't understand the date format. Task added without reminder.";
            }
        }

        private string ParseReminderDate(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("in") && lowerInput.Contains("day"))
            {
                Match match = Regex.Match(input, @"\d+");
                if (match.Success)
                {
                    int days = int.Parse(match.Value);
                    DateTime dueDate = DateTime.Now.AddDays(days);
                    return dueDate.ToString("MMM dd, yyyy");
                }
            }

            if (lowerInput.Contains("in") && lowerInput.Contains("week"))
            {
                Match match = Regex.Match(input, @"\d+");
                if (match.Success)
                {
                    int weeks = int.Parse(match.Value);
                    DateTime dueDate = DateTime.Now.AddDays(weeks * 7);
                    return dueDate.ToString("MMM dd, yyyy");
                }
            }

            try
            {
                DateTime parsedDate = DateTime.Parse(input.Trim());
                return parsedDate.ToString("MMM dd, yyyy");
            }
            catch
            {
                return null;
            }
        }

        private string AddTaskToDatabase(string taskName, string description, string reminderDate)
        {
            TaskModel newTask = new TaskModel
            {
                TaskName = taskName,
                TaskDescription = description,
                TaskStatus = "Pending"
            };

            if (!string.IsNullOrEmpty(reminderDate))
            {
                newTask.TaskDueDate = reminderDate;
            }

            if (dbHelper.InsertTask(newTask))
            {
                LoadTasks();
                if (!string.IsNullOrEmpty(reminderDate))
                {
                    return $"Task '{taskName}' added with reminder for {reminderDate}!";
                }
                else
                {
                    return $"Task '{taskName}' added successfully!";
                }
            }
            else
            {
                return "Failed to add task. Please try again.";
            }
        }

        private string GetTaskList()
        {
            LoadTasks();

            if (currentTasks.Count == 0)
            {
                return "You have no tasks. Add a task by saying 'add task' followed by the title.";
            }

            string result = "Your tasks:\n";
            int pendingCount = 0;
            int completedCount = 0;

            foreach (var task in currentTasks)
            {
                if (task.TaskStatus == "Pending")
                {
                    pendingCount++;
                    string dueDate = string.IsNullOrEmpty(task.TaskDueDate) ? "" : $" (Due: {task.TaskDueDate})";
                    result += $"○ Pending: {task.TaskName}{dueDate}\n";
                }
                else
                {
                    completedCount++;
                    result += $"✓ Completed: {task.TaskName}\n";
                }
            }

            result += $"\nTotal: {currentTasks.Count} tasks ({pendingCount} pending, {completedCount} completed)";
            return result;
        }

        private string HandleCompleteTask(string input)
        {
            LoadTasks();
            var pendingTasks = currentTasks.FindAll(t => t.TaskStatus == "Pending");

            if (pendingTasks.Count == 0)
            {
                return "You have no pending tasks to complete.";
            }

            string taskName = input;
            taskName = Regex.Replace(taskName, @"^(complete|finish|mark\s+done)\s+task\s*", "", RegexOptions.IgnoreCase);
            taskName = taskName.Trim();

            if (!string.IsNullOrEmpty(taskName))
            {
                foreach (var task in pendingTasks)
                {
                    if (task.TaskName.ToLower().Contains(taskName.ToLower()))
                    {
                        if (dbHelper.UpdateTaskStatus(task.TaskId, "Completed"))
                        {
                            LoadTasks();
                            return $"Task '{task.TaskName}' marked as completed!";
                        }
                        else
                        {
                            return "Failed to complete task. Please try again.";
                        }
                    }
                }
            }

            string taskList = "Which task would you like to complete?\n";
            for (int i = 0; i < pendingTasks.Count; i++)
            {
                taskList += $"{i + 1}. {pendingTasks[i].TaskName}\n";
            }
            taskList += "\nReply with the task name or number.";
            return taskList;
        }

        private string HandleDeleteTask(string input)
        {
            LoadTasks();

            if (currentTasks.Count == 0)
            {
                return "You have no tasks to delete.";
            }

            string taskName = input;
            taskName = Regex.Replace(taskName, @"^(delete|remove)\s+task\s*", "", RegexOptions.IgnoreCase);
            taskName = taskName.Trim();

            if (!string.IsNullOrEmpty(taskName))
            {
                foreach (var task in currentTasks)
                {
                    if (task.TaskName.ToLower().Contains(taskName.ToLower()))
                    {
                        if (dbHelper.DeleteTask(task.TaskId))
                        {
                            LoadTasks();
                            return $"Task '{task.TaskName}' deleted.";
                        }
                        else
                        {
                            return "Failed to delete task. Please try again.";
                        }
                    }
                }
            }

            string taskList = "Which task would you like to delete?\n";
            for (int i = 0; i < currentTasks.Count; i++)
            {
                taskList += $"{i + 1}. {currentTasks[i].TaskName}\n";
            }
            taskList += "\nReply with the task name or number.";
            return taskList;
        }

        public bool AddTaskFromUI(string taskName, string description, DateTime? reminderDate)
        {
            string reminder = reminderDate.HasValue ? reminderDate.Value.ToString("MMM dd, yyyy") : null;
            string result = AddTaskToDatabase(taskName, description, reminder);
            return result.Contains("added");
        }

        public bool CompleteTaskFromUI(int taskId)
        {
            return dbHelper.UpdateTaskStatus(taskId, "Completed");
        }

        public bool DeleteTaskFromUI(int taskId)
        {
            return dbHelper.DeleteTask(taskId);
        }

        public string GetTasksForDisplay()
        {
            LoadTasks();
            if (currentTasks.Count == 0)
            {
                return "No tasks found.";
            }

            string result = "";
            foreach (var task in currentTasks)
            {
                result += task.GetDisplayString() + "\n";
            }
            return result;
        }

        public bool IsWaitingForReminder()
        {
            return waitingForReminder;
        }

        public string GetPendingTaskName()
        {
            return pendingTaskName;
        }
    }
}