using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

        // Variables
        private string username = string.Empty;
        private string currentTopic = string.Empty;
        private int messageCount = 0;
        private Random random = new Random();

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

            // Play voice greeting
            new voice_greeting();
        }

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

            // Check for sentiment
            string sentiment = sentimentDetector.DetectSentiment(cleanInput);
            if (sentiment != "neutral")
            {
                string response = sentimentDetector.GetSentimentResponse(sentiment);
                AddMessage("jordan", response);
                return;
            }

            // Check for follow-up questions
            if (handler.IsFollowUpRequest(cleanInput) && !string.IsNullOrEmpty(currentTopic))
            {
                string response = finder.GetResponseByTopic(currentTopic);
                AddMessage("jordan", $"Here's more about {currentTopic}:\n\n{response}");
                return;
            }

            // Detect topic
            string detectedTopic = detector.DetectTopic(cleanInput);

            if (!string.IsNullOrEmpty(detectedTopic))
            {
                currentTopic = detectedTopic;
                string response = finder.GetResponseByTopic(detectedTopic);
                AddMessage("jordan", response);

                // Save interest if user shows interest
                if (cleanInput.Contains("interested in") || cleanInput.Contains("like learning"))
                {
                    SaveUserInterest(detectedTopic);
                }
            }
            else
            {
                string[] defaults = {
                    "I'm not sure I understand. Try asking about passwords, scams, or privacy.",
                    "Hmm, can you rephrase that? I can help with cybersecurity topics.",
                    "I don't recognize that. Ask me about online safety and security tips."
                };
                AddMessage("jordan", defaults[random.Next(defaults.Length)]);
            }

            messageCount++;
        }

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
                        AddMessage("jordan", $"I remember you're interested in {interests}. Want to learn more?");
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
    }
}