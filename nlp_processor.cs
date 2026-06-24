using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public class nlp_processor
    {
        private List<string> taskKeywords;
        private List<string> reminderKeywords;
        private List<string> quizKeywords;
        private List<string> logKeywords;
        private List<string> taskActionKeywords;
        private List<string> completionKeywords;
        private List<string> deletionKeywords;
        private Random random;

        public nlp_processor()
        {
            random = new Random();
            InitializeKeywords();
        }

        private void InitializeKeywords()
        {
            taskKeywords = new List<string>
            {
                "add task", "create task", "new task", "make task", "add a task",
                "create a task", "task for", "task about", "add", "create"
            };

            reminderKeywords = new List<string>
            {
                "remind me", "set reminder", "add reminder", "remind about",
                "remember to", "remind", "set a reminder", "create reminder"
            };

            quizKeywords = new List<string>
            {
                "start quiz", "take quiz", "play quiz", "quiz me",
                "test me", "cybersecurity quiz", "take a quiz"
            };

            logKeywords = new List<string>
            {
                "show log", "activity log", "what have you done",
                "recent actions", "show activity", "log", "history",
                "what did you do", "your actions", "summary"
            };

            taskActionKeywords = new List<string>
            {
                "for", "about", "to", "on", "regarding"
            };

            completionKeywords = new List<string>
            {
                "complete task", "mark done", "finish task", "done",
                "complete", "mark as complete", "task complete"
            };

            deletionKeywords = new List<string>
            {
                "delete task", "remove task", "delete", "remove",
                "erase task", "cancel task"
            };
        }

        public string ProcessNaturalLanguage(string input, task_manager taskManager, quiz_manager quizManager, string username)
        {
            string lowerInput = input.ToLower().Trim();

            if (ContainsKeyword(lowerInput, quizKeywords))
            {
                return "quiz";
            }

            if (ContainsKeyword(lowerInput, logKeywords))
            {
                return "log";
            }

            if (ContainsKeyword(lowerInput, completionKeywords))
            {
                return ExtractTaskForAction(input, "complete");
            }

            if (ContainsKeyword(lowerInput, deletionKeywords))
            {
                return ExtractTaskForAction(input, "delete");
            }

            if (ContainsKeyword(lowerInput, reminderKeywords))
            {
                return ExtractReminderInfo(input);
            }

            if (ContainsKeyword(lowerInput, taskKeywords))
            {
                return ExtractTaskInfo(input);
            }

            return null;
        }

        private bool ContainsKeyword(string input, List<string> keywords)
        {
            foreach (string keyword in keywords)
            {
                if (input.Contains(keyword))
                {
                    return true;
                }
            }
            return false;
        }

        private string ExtractTaskInfo(string input)
        {
            string lowerInput = input.ToLower();
            string taskText = input;

            foreach (string keyword in taskKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    int index = lowerInput.IndexOf(keyword);
                    if (index >= 0)
                    {
                        string before = input.Substring(0, index);
                        string after = input.Substring(index + keyword.Length);
                        taskText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            foreach (string word in taskActionKeywords)
            {
                if (taskText.ToLower().Contains(word))
                {
                    int index = taskText.ToLower().IndexOf(word);
                    if (index >= 0)
                    {
                        string before = taskText.Substring(0, index);
                        string after = taskText.Substring(index + word.Length);
                        taskText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            taskText = Regex.Replace(taskText, @"\s+", " ").Trim();

            if (string.IsNullOrEmpty(taskText) || taskText.Length < 2)
            {
                return null;
            }

            return taskText;
        }

        private string ExtractReminderInfo(string input)
        {
            string lowerInput = input.ToLower();
            string reminderText = input;

            foreach (string keyword in reminderKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    int index = lowerInput.IndexOf(keyword);
                    if (index >= 0)
                    {
                        string before = input.Substring(0, index);
                        string after = input.Substring(index + keyword.Length);
                        reminderText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            foreach (string word in taskActionKeywords)
            {
                if (reminderText.ToLower().Contains(word))
                {
                    int index = reminderText.ToLower().IndexOf(word);
                    if (index >= 0)
                    {
                        string before = reminderText.Substring(0, index);
                        string after = reminderText.Substring(index + word.Length);
                        reminderText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            reminderText = Regex.Replace(reminderText, @"\s+", " ").Trim();
            string dateInfo = ExtractDate(input);

            if (!string.IsNullOrEmpty(reminderText) && reminderText.Length > 2)
            {
                if (!string.IsNullOrEmpty(dateInfo))
                {
                    return $"reminder:{reminderText}|{dateInfo}";
                }
                return $"reminder:{reminderText}";
            }

            return null;
        }

        private string ExtractDate(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("tomorrow"))
            {
                return DateTime.Now.AddDays(1).ToString("MMM dd, yyyy");
            }

            if (lowerInput.Contains("today"))
            {
                return DateTime.Now.ToString("MMM dd, yyyy");
            }

            Match daysMatch = Regex.Match(lowerInput, @"in\s+(\d+)\s+days?");
            if (daysMatch.Success)
            {
                int days = int.Parse(daysMatch.Groups[1].Value);
                return DateTime.Now.AddDays(days).ToString("MMM dd, yyyy");
            }

            Match weeksMatch = Regex.Match(lowerInput, @"in\s+(\d+)\s+weeks?");
            if (weeksMatch.Success)
            {
                int weeks = int.Parse(weeksMatch.Groups[1].Value);
                return DateTime.Now.AddDays(weeks * 7).ToString("MMM dd, yyyy");
            }

            if (lowerInput.Contains("next week"))
            {
                return DateTime.Now.AddDays(7).ToString("MMM dd, yyyy");
            }

            if (lowerInput.Contains("next month"))
            {
                return DateTime.Now.AddMonths(1).ToString("MMM dd, yyyy");
            }

            try
            {
                Match dateMatch = Regex.Match(input, @"\d{4}-\d{2}-\d{2}|\d{2}/\d{2}/\d{4}|\d{2}-\d{2}-\d{4}");
                if (dateMatch.Success)
                {
                    DateTime parsedDate = DateTime.Parse(dateMatch.Value);
                    return parsedDate.ToString("MMM dd, yyyy");
                }
            }
            catch
            {
                // Ignore
            }

            return null;
        }

        private string ExtractTaskForAction(string input, string action)
        {
            string lowerInput = input.ToLower();
            string taskText = input;
            List<string> keywords = action == "complete" ? completionKeywords : deletionKeywords;

            foreach (string keyword in keywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    int index = lowerInput.IndexOf(keyword);
                    if (index >= 0)
                    {
                        string before = input.Substring(0, index);
                        string after = input.Substring(index + keyword.Length);
                        taskText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            foreach (string word in taskActionKeywords)
            {
                if (taskText.ToLower().Contains(word))
                {
                    int index = taskText.ToLower().IndexOf(word);
                    if (index >= 0)
                    {
                        string before = taskText.Substring(0, index);
                        string after = taskText.Substring(index + word.Length);
                        taskText = (before + " " + after).Trim();
                        break;
                    }
                }
            }

            taskText = Regex.Replace(taskText, @"\s+", " ").Trim();

            if (!string.IsNullOrEmpty(taskText) && taskText.Length > 2)
            {
                return $"{action}:{taskText}";
            }

            return $"{action}";
        }

        public string GetDefaultResponse()
        {
            string[] responses = {
                "I understand you are asking about cybersecurity. Can you be more specific about what you need help with?",
                "I am here to help with cybersecurity topics like passwords, phishing, and online safety. What would you like to know?",
                "I can help you with tasks, reminders, or answer cybersecurity questions. What do you need assistance with?",
                "You can ask me about cybersecurity topics, add tasks, set reminders, or start a quiz. How can I help you?"
            };
            return responses[random.Next(responses.Length)];
        }

        public string GetHelpResponse()
        {
            return @"I can help you with the following:

Cybersecurity Topics:
- Passwords and password safety
- Phishing and scam detection
- Safe browsing practices
- Social engineering awareness
- Malware protection
- Two-factor authentication
- Privacy protection

Task Management:
- 'Add task' to create a new task
- 'Complete task' to mark a task done
- 'Delete task' to remove a task
- 'Set reminder' to add a reminder

Other Features:
- 'Start quiz' to test your cybersecurity knowledge
- 'Show log' to see recent activity

Try rephrasing your question if I don't understand!";
        }
    }
}