using System;
using System.Collections.Generic;
using System.Linq;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public class quiz_manager
    {
        private List<QuizQuestion> questions;
        private List<QuizQuestion> shuffledQuestions;
        private int currentQuestionIndex;
        private int score;
        private bool isQuizActive;
        private Random random;
        private string currentAnswer = "";

        public event Action<string> OnQuestionDisplayed;
        public event Action<string> OnFeedbackGiven;
        public event Action<int, int, string> OnQuizCompleted;

        public bool IsQuizActive => isQuizActive;
        public int TotalQuestions => shuffledQuestions?.Count ?? 0;
        public int CurrentQuestionNumber => currentQuestionIndex + 1;
        public int CurrentScore => score;

        public quiz_manager()
        {
            random = new Random();
            InitializeQuestions();
            shuffledQuestions = new List<QuizQuestion>();
            isQuizActive = false;
            currentQuestionIndex = 0;
            score = 0;
        }

        private void InitializeQuestions()
        {
            questions = new List<QuizQuestion>();

            // Question 1: Password Safety (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 1,
                QuestionText = "What is the best practice for creating a strong password?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Use your birthday for easy recall",
                    "Use a combination of uppercase, lowercase, numbers, and symbols",
                    "Use the same password for all accounts",
                    "Use only numbers"
                },
                CorrectAnswerIndex = 1,
                Explanation = "A strong password should be at least 12 characters long and use a mix of uppercase, lowercase, numbers, and special symbols. This makes it harder for hackers to crack.",
                Topic = "Password Safety"
            });

            // Question 2: Phishing (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 2,
                QuestionText = "What should you do if you receive an email asking for your password?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Reply with your password",
                    "Delete the email",
                    "Report the email as phishing",
                    "Ignore it"
                },
                CorrectAnswerIndex = 2,
                Explanation = "Reporting phishing emails helps prevent scams. Legitimate companies will never ask for your password via email. Always report suspicious emails to your IT department or email provider.",
                Topic = "Phishing"
            });

            // Question 3: Safe Browsing (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionId = 3,
                QuestionText = "Using public Wi-Fi without a VPN is safe for online banking.",
                QuestionType = "TrueFalse",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Public Wi-Fi networks are often unencrypted, making it easy for hackers to intercept your data. Always use a VPN when accessing sensitive information on public networks.",
                Topic = "Safe Browsing"
            });

            // Question 4: Social Engineering (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 4,
                QuestionText = "What is social engineering in cybersecurity?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Building social networks for business",
                    "Manipulating people into revealing confidential information",
                    "Creating social media accounts",
                    "Writing social media posts"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Social engineering is the psychological manipulation of people to divulge confidential information. Attackers exploit human trust rather than technical vulnerabilities.",
                Topic = "Social Engineering"
            });

            // Question 5: Two-Factor Authentication (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionId = 5,
                QuestionText = "Two-Factor Authentication (2FA) provides an extra layer of security beyond just a password.",
                QuestionType = "TrueFalse",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 0,
                Explanation = "2FA adds a second verification step, such as a code sent to your phone. Even if your password is stolen, the attacker cannot access your account without the second factor.",
                Topic = "Authentication"
            });

            // Question 6: Malware (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 6,
                QuestionText = "Which of the following is a type of malware that holds your files hostage?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Virus",
                    "Ransomware",
                    "Spyware",
                    "Adware"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Ransomware encrypts your files and demands payment for the decryption key. Always back up your important files and avoid clicking suspicious links to prevent ransomware attacks.",
                Topic = "Malware"
            });

            // Question 7: Phishing (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionId = 7,
                QuestionText = "It is safe to click on links in emails from unknown senders if they offer a great deal.",
                QuestionType = "TrueFalse",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Never click links from unknown senders, even if the offer seems legitimate. These links often lead to phishing sites that steal your personal information.",
                Topic = "Phishing"
            });

            // Question 8: Password Safety (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 8,
                QuestionText = "What is a passphrase compared to a traditional password?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "It is shorter and easier to crack",
                    "It is longer and uses multiple words, making it stronger",
                    "It only uses numbers",
                    "It is the same as a password"
                },
                CorrectAnswerIndex = 1,
                Explanation = "A passphrase like 'PurpleElephantDancesAtMidnight' is longer and uses multiple words, making it both stronger and easier to remember than a complex password.",
                Topic = "Password Safety"
            });

            // Question 9: Safe Browsing (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 9,
                QuestionText = "What should you look for to ensure a website is secure?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "The website looks professional",
                    "https:// and a padlock icon in the address bar",
                    "The website loads quickly",
                    "The website has many ads"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Always look for 'https://' and the padlock icon in the address bar. This indicates the connection is encrypted and your data is protected during transmission.",
                Topic = "Safe Browsing"
            });

            // Question 10: Social Engineering (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionId = 10,
                QuestionText = "It is safe to share your OTP (One-Time Password) with someone claiming to be from your bank.",
                QuestionType = "TrueFalse",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Never share your OTP with anyone. Legitimate banks will never ask for your OTP over the phone, email, or SMS. This is a common social engineering tactic.",
                Topic = "Social Engineering"
            });

            // Question 11: Malware (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 11,
                QuestionText = "How can you protect your computer from malware?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Download software from any website",
                    "Keep your antivirus software updated",
                    "Disable your firewall",
                    "Use only free software"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Keep your antivirus software updated and run regular scans. Also, only download software from official websites and avoid clicking suspicious links.",
                Topic = "Malware"
            });

            // Question 12: Privacy (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 12,
                QuestionText = "What is the best way to protect your privacy on social media?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "Share everything publicly",
                    "Use privacy settings to limit who can see your information",
                    "Accept all friend requests",
                    "Post your phone number"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Always review and adjust your privacy settings on social media. Limit who can see your posts, personal information, and contact details to protect your privacy.",
                Topic = "Privacy"
            });

            // Question 13: South Africa Specific (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 13,
                QuestionText = "What is 'smishing' that is common in South Africa?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "A type of malware",
                    "SMS phishing scams",
                    "A secure messaging app",
                    "Two-factor authentication"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Smishing is SMS phishing where scammers send text messages pretending to be from banks like Capitec, FNB, or Standard Bank to steal your personal information.",
                Topic = "South Africa"
            });

            // Question 14: Two-Factor Authentication (Multiple Choice)
            questions.Add(new QuizQuestion
            {
                QuestionId = 14,
                QuestionText = "Which of the following is NOT a form of Two-Factor Authentication?",
                QuestionType = "MultipleChoice",
                Options = new List<string> {
                    "SMS code",
                    "Authenticator app code",
                    "A security question",
                    "Biometric verification"
                },
                CorrectAnswerIndex = 2,
                Explanation = "Security questions are not considered 2FA because they are just another form of 'something you know'. 2FA requires 'something you have' (phone, authenticator) or 'something you are' (biometrics).",
                Topic = "Authentication"
            });

            // Question 15: Phishing (True/False)
            questions.Add(new QuizQuestion
            {
                QuestionId = 15,
                QuestionText = "Phishing emails often create a sense of urgency to trick you into acting quickly.",
                QuestionType = "TrueFalse",
                Options = new List<string> { "True", "False" },
                CorrectAnswerIndex = 0,
                Explanation = "Phishing emails often use urgent language like 'Your account will be closed' or 'Immediate action required' to rush you into making mistakes without thinking.",
                Topic = "Phishing"
            });
        }

        public string StartQuiz()
        {
            if (questions.Count == 0)
            {
                return "No questions available. Please try again later.";
            }

            // Shuffle questions
            shuffledQuestions = questions.OrderBy(x => random.Next()).ToList();
            currentQuestionIndex = 0;
            score = 0;
            isQuizActive = true;

            return GetNextQuestion();
        }

        public string GetNextQuestion()
        {
            if (currentQuestionIndex >= shuffledQuestions.Count)
            {
                return EndQuiz();
            }

            var question = shuffledQuestions[currentQuestionIndex];
            string displayText = question.GetDisplayText();

            string message = $"Question {currentQuestionIndex + 1} of {shuffledQuestions.Count} ({question.Topic}):\n\n{displayText}";

            OnQuestionDisplayed?.Invoke(message);
            return message;
        }

        public string SubmitAnswer(string answer)
        {
            if (!isQuizActive)
            {
                return "The quiz is not active. Type 'start quiz' to begin.";
            }

            if (currentQuestionIndex >= shuffledQuestions.Count)
            {
                return EndQuiz();
            }

            var question = shuffledQuestions[currentQuestionIndex];
            currentAnswer = answer;

            bool isCorrect = question.IsCorrect(answer);

            if (isCorrect)
            {
                score++;
                string feedback = $"Correct! {question.GetExplanationWithCorrectAnswer()}";
                OnFeedbackGiven?.Invoke(feedback);
                currentQuestionIndex++;
                return feedback;
            }
            else
            {
                string feedback = $"Incorrect. {question.GetExplanationWithCorrectAnswer()}";
                OnFeedbackGiven?.Invoke(feedback);
                currentQuestionIndex++;
                return feedback;
            }
        }

        public string EndQuiz()
        {
            isQuizActive = false;

            int totalQuestions = shuffledQuestions.Count;
            int correctAnswers = score;
            double percentage = (double)correctAnswers / totalQuestions * 100;

            string feedback = "";

            if (percentage >= 90)
            {
                feedback = "Excellent! You are a cybersecurity pro!";
            }
            else if (percentage >= 70)
            {
                feedback = "Great job! You have good cybersecurity knowledge.";
            }
            else if (percentage >= 50)
            {
                feedback = "Good effort! Keep learning to stay safe online.";
            }
            else
            {
                feedback = "Keep learning! Cybersecurity is important for everyone.";
            }

            string result = $"Quiz Complete!\n\nScore: {correctAnswers} out of {totalQuestions} ({percentage:F0}%)\n\n{feedback}";

            OnQuizCompleted?.Invoke(correctAnswers, totalQuestions, feedback);
            return result;
        }

        public string ProcessQuizInput(string input)
        {
            if (!isQuizActive)
            {
                // Check if user wants to start quiz
                string lowerInput = input.ToLower().Trim();
                if (lowerInput.Contains("start quiz") || lowerInput.Contains("take quiz") ||
                    lowerInput.Contains("play quiz") || lowerInput.Contains("quiz me"))
                {
                    return StartQuiz();
                }
                return null;
            }

            // User is in quiz - submit answer
            return SubmitAnswer(input);
        }

        public bool IsWaitingForAnswer()
        {
            return isQuizActive;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (isQuizActive && currentQuestionIndex < shuffledQuestions.Count)
            {
                return shuffledQuestions[currentQuestionIndex];
            }
            return null;
        }
    }
}