using System;

namespace cybersecurity_chatbot_p2.Models
{
    public class ActivityLogEntry
    {
        public int LogId { get; set; }
        public string ActionType { get; set; }
        public string ActionDescription { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string UserName { get; set; }

        public ActivityLogEntry()
        {
            ActionTimestamp = DateTime.Now;
        }

        public ActivityLogEntry(string actionType, string description, string userName = null)
        {
            ActionType = actionType;
            ActionDescription = description;
            ActionTimestamp = DateTime.Now;
            UserName = userName;
        }

        public override string ToString()
        {
            return $"[{ActionTimestamp:yyyy-MM-dd HH:mm}] {ActionType}: {ActionDescription}";
        }

        public string ToShortString()
        {
            return $"{ActionType}: {ActionDescription}";
        }
    }
}