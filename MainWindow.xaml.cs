using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public partial class MainWindow : Window
    {
        // Collections for responses
        private ArrayList reply = new ArrayList();
        private ArrayList ignore = new ArrayList();

        // Class instances
        private response_finder finder;
        private response_handler handler;
        private topic_detector detector;
        private sentiment_detector sentimentDetector;
        private task_manager taskManager;
        private quiz_manager quizManager;
        private nlp_processor nlpProcessor;
        private activity_logger activityLogger;

        // Variables
        private string username = string.Empty;
        private string currentTopic = string.Empty;
        private int messageCount = 0;
        private Random random = new Random();
        private TaskModel selectedTask = null;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize responses
            new respond(reply, ignore);

            // Initialize all helper classes
            finder = new response_finder(reply);
            handler = new response_handler(reply, ignore);
            detector = new topic_detector();
            sentimentDetector = new sentiment_detector(reply, finder);

            // Initialize Task Manager
            taskManager = new task_manager();

            // Initialize Quiz Manager
            quizManager = new quiz_manager();

            // Initialize NLP Processor
            nlpProcessor = new nlp_processor();

            // Initialize Activity Logger
            activityLogger = new activity_logger();

            // Subscribe to quiz events
            quizManager.OnQuestionDisplayed += (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    QuizQuestionDisplay.Text = message;
                    UpdateQuizProgress();
                });
            };

            quizManager.OnFeedbackGiven += (feedback) =>
            {
                Dispatcher.Invoke(() =>
                {
                    QuizQuestionDisplay.Text = feedback;
                    UpdateQuizProgress();
                });
            };

            quizManager.OnQuizCompleted += (score, total, feedback) =>
            {
                Dispatcher.Invoke(() =>
                {
                    QuizScoreDisplay.Text = $"Score: {score}/{total}";
                    UpdateQuizProgress();
                    activityLogger.LogQuizCompleted(score, total);
                });
            };

            // Play voice greeting
            new voice_greeting();

            // Log system startup
            activityLogger.LogSystemStartup();
        }

        // ============= NAVIGATION METHODS =============

        private void TasksTabButton_Click(object sender, RoutedEventArgs e)
        {
            TasksPanel.Visibility = Visibility.Visible;
            QuizPanel.Visibility = Visibility.Hidden;
            TasksTabButton.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
            QuizTabButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
        }

        private void QuizTabButton_Click(object sender, RoutedEventArgs e)
        {
            TasksPanel.Visibility = Visibility.Hidden;
            QuizPanel.Visibility = Visibility.Visible;
            QuizTabButton.Background = new SolidColorBrush(Color.FromRgb(243, 156, 18));
            TasksTabButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
        }

        // ============= QUIZ METHODS =============

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            string response = quizManager.StartQuiz();
            QuizScoreDisplay.Text = "Score: 0/0";
            AddMessage("jordan", response);
            activityLogger.LogQuizStarted();

            if (quizManager.TotalQuestions > 0)
            {
                QuizProgressBar.Maximum = quizManager.TotalQuestions;
            }
            UpdateQuizProgress();
        }

        private void UpdateQuizProgress()
        {
            if (quizManager.TotalQuestions > 0)
            {
                double progress = (double)quizManager.CurrentQuestionNumber / quizManager.TotalQuestions * 100;
                QuizProgressBar.Value = Math.Min(progress, 100);
                QuizProgressText.Text = $"{Math.Min(progress, 100):F0}% Complete";
            }

            if (quizManager.IsQuizActive)
            {
                QuizStatusDisplay.Text = "Quiz in progress";
                QuizStatusDisplay.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                QuizStatusDisplay.Text = "";
            }
        }

        // ============= MAIN METHODS =============

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string enteredName = usernames_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(enteredName))
            {
                errorMessage.Visibility = Visibility.Visible;
                return;
            }

            username = enteredName;
            errorMessage.Visibility = Visibility.Collapsed;

            // Set user in activity logger
            activityLogger.SetUser(username);

            // Save user to file
            SaveUserToFile();

            // Show chat interface
            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            // Check if user exists and show appropriate welcome message
            if (IsExistingUser(username))
            {
                AddMessage("jordan", $"Hey {username} welcome back, how can i help you today");
            }
            else
            {
                AddMessage("jordan", $"Hey {username} welcome to AI cybersecurity");
            }

            // Recall previous interests
            RecallUserInterests();

            // Load tasks
            LoadTasksUI();

            // Log user login
            activityLogger.LogUserLogin(username);
        }

        private bool IsExistingUser(string name)
        {
            string filename = "users.txt";
            if (File.Exists(filename))
            {
                string[] users = File.ReadAllLines(filename);
                return users.Contains(name);
            }
            return false;
        }

        private void send(object sender, RoutedEventArgs e)
        {
            string userInput = question.Text.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
                return;

            // Show user message
            AddMessage(username, userInput);

            // Process the input
            ProcessUserInput(userInput);

            // Clear input
            question.Clear();
        }

        private void ProcessUserInput(string input)
        {
            string cleanInput = RemoveSpecialCharacters(input);

            // ============= CHECK FOR LOG COMMAND =============
            string lowerInput = input.ToLower().Trim();

            if (lowerInput.Contains("show log") || lowerInput.Contains("activity log") ||
                lowerInput.Contains("what have you done") || lowerInput.Contains("recent actions") ||
                lowerInput.Contains("what did you do") || lowerInput.Contains("show activity") ||
                lowerInput.Contains("your actions") || lowerInput.Contains("summary"))
            {
                // Check if user wants full log or specific type
                if (lowerInput.Contains("full") || lowerInput.Contains("all") || lowerInput.Contains("complete"))
                {
                    ShowFullActivityLog();
                }
                else if (lowerInput.Contains("task") && lowerInput.Contains("log"))
                {
                    ShowLogByType("Task");
                }
                else if (lowerInput.Contains("quiz") && lowerInput.Contains("log"))
                {
                    ShowLogByType("Quiz");
                }
                else if (lowerInput.Contains("reminder") && lowerInput.Contains("log"))
                {
                    ShowLogByType("Reminder");
                }
                else if (lowerInput.Contains("nlp") && lowerInput.Contains("log"))
                {
                    ShowLogByType("NLP");
                }
                else
                {
                    ShowActivityLog(10);
                }
                return;
            }

            // ============= NLP PROCESSING =============
            if (nlpProcessor != null)
            {
                string nlpResult = nlpProcessor.ProcessNaturalLanguage(input, taskManager, quizManager, username);

                if (nlpResult != null)
                {
                    // Handle different NLP intents
                    if (nlpResult == "quiz")
                    {
                        string response = quizManager.StartQuiz();
                        if (response != null)
                        {
                            AddMessage("jordan", response);
                            activityLogger.LogQuizStarted();
                            QuizScoreDisplay.Text = "Score: 0/0";
                            if (quizManager.TotalQuestions > 0)
                            {
                                QuizProgressBar.Maximum = quizManager.TotalQuestions;
                            }
                            UpdateQuizProgress();
                            return;
                        }
                    }
                    else if (nlpResult == "log")
                    {
                        ShowActivityLog(10);
                        return;
                    }
                    else if (nlpResult.StartsWith("complete:"))
                    {
                        string taskName = nlpResult.Substring(9).Trim();
                        if (!string.IsNullOrEmpty(taskName) && taskName.Length > 2)
                        {
                            HandleTaskCompletionByName(taskName);
                            return;
                        }
                        else
                        {
                            AddMessage("jordan", "Which task would you like to complete? Please specify the task name.");
                            return;
                        }
                    }
                    else if (nlpResult.StartsWith("delete:"))
                    {
                        string taskName = nlpResult.Substring(7).Trim();
                        if (!string.IsNullOrEmpty(taskName) && taskName.Length > 2)
                        {
                            HandleTaskDeletionByName(taskName);
                            return;
                        }
                        else
                        {
                            AddMessage("jordan", "Which task would you like to delete? Please specify the task name.");
                            return;
                        }
                    }
                    else if (nlpResult.StartsWith("reminder:"))
                    {
                        HandleReminderNLP(nlpResult);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(nlpResult))
                    {
                        HandleTaskCreationNLP(nlpResult);
                        return;
                    }
                }
            }

            // ============= QUIZ PROCESSING =============
            if (quizManager != null)
            {
                if (quizManager.IsQuizActive)
                {
                    string quizResponse = quizManager.ProcessQuizInput(input);
                    if (quizResponse != null)
                    {
                        AddMessage("jordan", quizResponse);

                        if (quizManager.IsQuizActive)
                        {
                            QuizScoreDisplay.Text = $"Score: {quizManager.CurrentScore}/{quizManager.CurrentQuestionNumber}";
                        }
                        else
                        {
                            QuizScoreDisplay.Text = $"Score: {quizManager.CurrentScore}/{quizManager.TotalQuestions}";
                        }

                        UpdateQuizProgress();
                        return;
                    }
                }
            }

            // ============= TASK MANAGER PROCESSING =============
            if (taskManager != null)
            {
                string taskResponse = taskManager.ProcessTaskInput(input);
                if (taskResponse != null)
                {
                    if (taskManager.IsWaitingForReminder())
                    {
                        AddMessage("jordan", taskResponse);
                        return;
                    }

                    AddMessage("jordan", taskResponse);
                    LoadTasksUI();
                    return;
                }
            }

            // Check for sentiment
            string sentiment = sentimentDetector.DetectSentiment(cleanInput);
            if (sentiment != "neutral")
            {
                string response = sentimentDetector.GetSentimentResponse(sentiment);
                AddMessage("jordan", response);
                activityLogger.LogSentimentDetected(sentiment);
                return;
            }

            // Check for follow-up questions
            if (handler.IsFollowUpRequest(cleanInput) && !string.IsNullOrEmpty(currentTopic))
            {
                string response = finder.GetResponseByTopic(currentTopic);
                AddMessage("jordan", $"Here is more about {currentTopic}:\n\n{response}");
                activityLogger.LogTopicDiscussed(currentTopic);
                return;
            }

            // Check for help
            if (cleanInput.Contains("help") || cleanInput.Contains("what can you do"))
            {
                string helpResponse = nlpProcessor.GetHelpResponse();
                AddMessage("jordan", helpResponse);
                return;
            }

            // Detect topic
            string detectedTopic = detector.DetectTopic(cleanInput);

            if (!string.IsNullOrEmpty(detectedTopic))
            {
                currentTopic = detectedTopic;
                string response = finder.GetResponseByTopic(detectedTopic);
                AddMessage("jordan", response);

                if (cleanInput.Contains("interested in") || cleanInput.Contains("like learning"))
                {
                    SaveUserInterest(detectedTopic);
                }

                activityLogger.LogTopicDiscussed(detectedTopic);
            }
            else
            {
                string defaultResponse = nlpProcessor.GetDefaultResponse();
                AddMessage("jordan", defaultResponse);
            }

            messageCount++;
        }

        // ============= ACTIVITY LOG METHODS =============

        private void ShowActivityLog(int count = 10)
        {
            string logSummary = activityLogger.GetLogSummary(count);
            AddMessage("jordan", logSummary);
        }

        private void ShowFullActivityLog()
        {
            string logSummary = activityLogger.GetFullLogSummary();
            AddMessage("jordan", logSummary);
        }

        private void ShowLogByType(string actionType)
        {
            string logSummary = activityLogger.GetLogByType(actionType);
            AddMessage("jordan", logSummary);
        }

        // ============= NLP HELPER METHODS =============

        private void HandleTaskCreationNLP(string taskName)
        {
            if (taskManager != null)
            {
                var existingTasks = taskManager.GetTasks();
                foreach (var task in existingTasks)
                {
                    if (task.TaskName.ToLower() == taskName.ToLower())
                    {
                        AddMessage("jordan", $"A task with the name '{taskName}' already exists. Would you like to create a different task?");
                        return;
                    }
                }

                if (taskManager.AddTaskFromUI(taskName, "Created via NLP", null))
                {
                    AddMessage("jordan", $"Task '{taskName}' added successfully! Would you like to set a reminder for this task?");
                    LoadTasksUI();
                    activityLogger.LogTaskAdded(taskName);
                }
                else
                {
                    AddMessage("jordan", "Failed to add task. Please try again.");
                }
            }
        }

        private void HandleReminderNLP(string reminderInfo)
        {
            string[] parts = reminderInfo.Split('|');
            string reminderText = parts[0].Replace("reminder:", "");
            string dateInfo = parts.Length > 1 ? parts[1] : null;

            if (taskManager != null)
            {
                var tasks = taskManager.GetTasks();
                bool taskFound = false;

                foreach (var task in tasks)
                {
                    if (task.TaskName.ToLower().Contains(reminderText.ToLower()))
                    {
                        taskFound = true;
                        string reminderDate = dateInfo ?? DateTime.Now.AddDays(7).ToString("MMM dd, yyyy");
                        AddMessage("jordan", $"Reminder set for '{task.TaskName}' on {reminderDate}");
                        activityLogger.LogReminderSet(task.TaskName, reminderDate);
                        return;
                    }
                }

                if (!taskFound)
                {
                    string reminderDate = dateInfo ?? DateTime.Now.AddDays(7).ToString("MMM dd, yyyy");
                    if (taskManager.AddTaskFromUI(reminderText, $"Reminder task", DateTime.Parse(reminderDate)))
                    {
                        AddMessage("jordan", $"Task '{reminderText}' added with reminder for {reminderDate}!");
                        LoadTasksUI();
                        activityLogger.LogTaskAdded(reminderText);
                        activityLogger.LogReminderSet(reminderText, reminderDate);
                    }
                    else
                    {
                        AddMessage("jordan", "Failed to add task with reminder. Please try again.");
                    }
                }
            }
        }

        private void HandleTaskCompletionByName(string taskName)
        {
            if (taskManager != null)
            {
                var tasks = taskManager.GetTasks();
                bool taskFound = false;

                foreach (var task in tasks)
                {
                    if (task.TaskName.ToLower().Contains(taskName.ToLower()) && task.TaskStatus == "Pending")
                    {
                        if (taskManager.CompleteTaskFromUI(task.TaskId))
                        {
                            AddMessage("jordan", $"Task '{task.TaskName}' marked as completed!");
                            LoadTasksUI();
                            activityLogger.LogTaskCompleted(task.TaskName);
                            taskFound = true;
                            break;
                        }
                    }
                }

                if (!taskFound)
                {
                    AddMessage("jordan", $"Could not find a pending task matching '{taskName}'. Please check the task name and try again.");
                }
            }
        }

        private void HandleTaskDeletionByName(string taskName)
        {
            if (taskManager != null)
            {
                var tasks = taskManager.GetTasks();
                bool taskFound = false;

                foreach (var task in tasks)
                {
                    if (task.TaskName.ToLower().Contains(taskName.ToLower()))
                    {
                        if (taskManager.DeleteTaskFromUI(task.TaskId))
                        {
                            AddMessage("jordan", $"Task '{task.TaskName}' deleted.");
                            LoadTasksUI();
                            activityLogger.LogTaskDeleted(task.TaskName);
                            taskFound = true;
                            break;
                        }
                    }
                }

                if (!taskFound)
                {
                    AddMessage("jordan", $"Could not find a task matching '{taskName}'. Please check the task name and try again.");
                }
            }
        }

        // ============= EXISTING METHODS =============

        private void AddMessage(string sender, string message)
        {
            Dispatcher.Invoke(() =>
            {
                Border border = new Border
                {
                    Margin = new Thickness(5, 8, 5, 8),
                    Padding = new Thickness(10, 8, 10, 8),
                    CornerRadius = new CornerRadius(8)
                };

                if (sender.ToLower() == "jordan")
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230));
                    border.BorderThickness = new Thickness(1);
                    border.HorizontalAlignment = HorizontalAlignment.Left;
                    border.MaxWidth = 500;
                }
                else
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(220, 248, 220));
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(144, 238, 144));
                    border.BorderThickness = new Thickness(1);
                    border.HorizontalAlignment = HorizontalAlignment.Right;
                    border.MaxWidth = 500;
                }

                TextBlock text = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 14,
                    Foreground = Brushes.Black
                };

                text.Text = $"{sender}: {message}";
                border.Child = text;

                chats.Items.Add(border);
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            });
        }

        private void SaveUserToFile()
        {
            string filename = "users.txt";
            if (!File.Exists(filename))
                File.WriteAllText(filename, "");

            string[] users = File.ReadAllLines(filename);
            if (!users.Contains(username))
                File.AppendAllText(filename, username + "\n");
        }

        private void SaveUserInterest(string interest)
        {
            string filename = "user_interests.txt";
            if (!File.Exists(filename))
                File.WriteAllText(filename, "");

            var lines = File.ReadAllLines(filename).ToList();
            bool found = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(username + ":"))
                {
                    if (!lines[i].Contains(interest))
                        lines[i] = lines[i] + ", " + interest;
                    found = true;
                    break;
                }
            }

            if (!found)
                lines.Add(username + ":" + interest);

            File.WriteAllLines(filename, lines);
        }

        private void RecallUserInterests()
        {
            string filename = "user_interests.txt";
            if (File.Exists(filename))
            {
                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    if (line.StartsWith(username + ":"))
                    {
                        string interests = line.Substring(line.IndexOf(":") + 1);
                        AddMessage("jordan", $"I remember you are interested in {interests}. Want to learn more?");
                        break;
                    }
                }
            }
        }

        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            StringBuilder result = new StringBuilder();
            foreach (char c in input.ToLower())
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    result.Append(c);
                else
                    result.Append(' ');
            }

            return Regex.Replace(result.ToString(), @"\s+", " ").Trim();
        }

        private void question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                send(sender, null);
        }

        // ============= TASK UI METHODS =============

        private void LoadTasksUI()
        {
            if (taskManager != null)
            {
                var tasks = taskManager.GetTasks();
                TasksListBox.Items.Clear();

                if (tasks.Count == 0)
                {
                    TasksListBox.Items.Add("No tasks found.");
                }
                else
                {
                    foreach (var task in tasks)
                    {
                        TasksListBox.Items.Add(task.GetDisplayString());
                    }
                }

                TaskCountDisplay.Text = $"{taskManager.GetPendingCount()} pending, {taskManager.GetCompletedCount()} completed";
            }
        }

        private void TasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksListBox.SelectedIndex >= 0 && taskManager != null)
            {
                var tasks = taskManager.GetTasks();
                if (TasksListBox.SelectedIndex < tasks.Count)
                {
                    selectedTask = tasks[TasksListBox.SelectedIndex];
                }
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || title == "Enter task title...")
            {
                MessageBox.Show("Please enter a task title.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime? reminderDate = null;
            if (SetReminderCheck.IsChecked == true && ReminderDatePicker.SelectedDate.HasValue)
            {
                reminderDate = ReminderDatePicker.SelectedDate.Value;
            }

            if (taskManager != null && taskManager.AddTaskFromUI(title, description, reminderDate))
            {
                AddMessage("jordan", $"Task '{title}' added successfully!");
                LoadTasksUI();

                if (reminderDate.HasValue)
                {
                    activityLogger.LogTaskAdded(title);
                    activityLogger.LogReminderSet(title, reminderDate.Value.ToString("MMM dd, yyyy"));
                }
                else
                {
                    activityLogger.LogTaskAdded(title);
                }

                TaskTitleBox.Text = "Enter task title...";
                TaskDescriptionBox.Text = "Enter description...";
                SetReminderCheck.IsChecked = false;
                ReminderDatePicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                AddMessage("jordan", "Failed to add task. Please try again.");
            }
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                AddMessage("jordan", "Please select a task to complete.");
                return;
            }

            if (selectedTask.TaskStatus == "Completed")
            {
                AddMessage("jordan", $"Task '{selectedTask.TaskName}' is already completed.");
                return;
            }

            if (taskManager != null && taskManager.CompleteTaskFromUI(selectedTask.TaskId))
            {
                AddMessage("jordan", $"Task '{selectedTask.TaskName}' marked as completed!");
                LoadTasksUI();
                activityLogger.LogTaskCompleted(selectedTask.TaskName);
                selectedTask = null;
            }
            else
            {
                AddMessage("jordan", "Failed to complete task. Please try again.");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                AddMessage("jordan", "Please select a task to delete.");
                return;
            }

            var result = MessageBox.Show($"Delete task '{selectedTask.TaskName}'?",
                                        "Confirm Delete",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (taskManager != null && taskManager.DeleteTaskFromUI(selectedTask.TaskId))
                {
                    AddMessage("jordan", $"Task '{selectedTask.TaskName}' deleted.");
                    LoadTasksUI();
                    activityLogger.LogTaskDeleted(selectedTask.TaskName);
                    selectedTask = null;
                }
                else
                {
                    AddMessage("jordan", "Failed to delete task. Please try again.");
                }
            }
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasksUI();
            AddMessage("jordan", "Task list refreshed.");
        }

        private void SetReminderCheck_Checked(object sender, RoutedEventArgs e)
        {
            ReminderDatePicker.Visibility = Visibility.Visible;
            ReminderDatePicker.SelectedDate = DateTime.Now.AddDays(7);
        }

        private void SetReminderCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            ReminderDatePicker.Visibility = Visibility.Collapsed;
        }

        // ============= WINDOW CLOSING =============

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            activityLogger.LogSystemShutdown();
        }
    }
}