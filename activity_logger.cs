using System;
using System.Collections.Generic;
using System.Linq;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public class activity_logger
    {
        private List<ActivityLogEntry> logEntries;
        private int maxEntries;
        private string currentUser;

        public activity_logger(string userName = null)
        {
            logEntries = new List<ActivityLogEntry>();
            maxEntries = 100;
            currentUser = userName ?? "DefaultUser";
        }

        public void SetUser(string userName)
        {
            currentUser = userName;
        }

        public void LogActivity(string actionType, string description)
        {
            var entry = new ActivityLogEntry(actionType, description, currentUser);
            logEntries.Insert(0, entry);

            while (logEntries.Count > maxEntries)
            {
                logEntries.RemoveAt(logEntries.Count - 1);
            }
        }

        public List<ActivityLogEntry> GetRecentActivities(int count = 10)
        {
            int takeCount = Math.Min(count, logEntries.Count);
            return logEntries.Take(takeCount).ToList();
        }

        public List<ActivityLogEntry> GetAllActivities()
        {
            return new List<ActivityLogEntry>(logEntries);
        }

        public string GetLogSummary(int count = 10)
        {
            var activities = GetRecentActivities(count);

            if (activities.Count == 0)
            {
                return "No activities have been logged yet.";
            }

            string summary = "Here is a summary of recent actions:\n";
            int index = 1;

            foreach (var activity in activities)
            {
                summary += $"{index}. {activity}\n";
                index++;
            }

            return summary;
        }

        public string GetFullLogSummary()
        {
            if (logEntries.Count == 0)
            {
                return "No activities have been logged yet.";
            }

            string summary = "Complete Activity Log:\n";
            int index = 1;

            foreach (var activity in logEntries)
            {
                summary += $"{index}. {activity}\n";
                index++;
            }

            return summary;
        }

        public string GetLogByType(string actionType)
        {
            var filtered = logEntries.Where(e => e.ActionType.ToLower() == actionType.ToLower()).ToList();

            if (filtered.Count == 0)
            {
                return $"No activities of type '{actionType}' found.";
            }

            string summary = $"Activities of type '{actionType}':\n";
            int index = 1;

            foreach (var activity in filtered)
            {
                summary += $"{index}. {activity}\n";
                index++;
            }

            return summary;
        }

        public int GetLogCount()
        {
            return logEntries.Count;
        }

        public void ClearLog()
        {
            logEntries.Clear();
            LogActivity("System", "Activity log cleared");
        }

        // ============= CONVENIENCE METHODS =============

        public void LogTaskAdded(string taskName)
        {
            LogActivity("Task", $"Task added: '{taskName}'");
        }

        public void LogTaskCompleted(string taskName)
        {
            LogActivity("Task", $"Task completed: '{taskName}'");
        }

        public void LogTaskDeleted(string taskName)
        {
            LogActivity("Task", $"Task deleted: '{taskName}'");
        }

        public void LogReminderSet(string taskName, string reminderDate)
        {
            LogActivity("Reminder", $"Reminder set for '{taskName}' on {reminderDate}");
        }

        public void LogQuizStarted()
        {
            LogActivity("Quiz", "Quiz started");
        }

        public void LogQuizCompleted(int score, int total)
        {
            LogActivity("Quiz", $"Quiz completed with score {score}/{total}");
        }

        public void LogNLPInteraction(string userInput, string intent)
        {
            LogActivity("NLP", $"Intent '{intent}' detected from: '{userInput}'");
        }

        public void LogSentimentDetected(string sentiment)
        {
            LogActivity("Sentiment", $"User expressed {sentiment} sentiment");
        }

        public void LogTopicDiscussed(string topic)
        {
            LogActivity("Topic", $"User asked about '{topic}'");
        }

        public void LogUserLogin(string userName)
        {
            LogActivity("User", $"User '{userName}' logged in");
        }

        public void LogSystemStartup()
        {
            LogActivity("System", "Chatbot started");
        }

        public void LogSystemShutdown()
        {
            LogActivity("System", "Chatbot shutdown");
        }
    }
}