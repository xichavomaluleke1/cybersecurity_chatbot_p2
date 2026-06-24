using System;
using System.Collections.Generic;

namespace cybersecurity_chatbot_p2.Models
{
    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "MultipleChoice" or "TrueFalse"
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; }
        public string Topic { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
        }

        public string GetDisplayText()
        {
            string display = $"Q{QuestionId}: {QuestionText}\n";

            if (QuestionType == "MultipleChoice")
            {
                char[] labels = { 'A', 'B', 'C', 'D' };
                for (int i = 0; i < Options.Count; i++)
                {
                    display += $"{labels[i]}) {Options[i]}\n";
                }
            }
            else if (QuestionType == "TrueFalse")
            {
                display += "A) True\nB) False\n";
            }

            return display;
        }

        public bool IsCorrect(string answer)
        {
            string lowerAnswer = answer.ToLower().Trim();

            // Handle letter answers (A, B, C, D)
            if (lowerAnswer.Length == 1 && lowerAnswer[0] >= 'a' && lowerAnswer[0] <= 'd')
            {
                int selectedIndex = lowerAnswer[0] - 'a';
                return selectedIndex == CorrectAnswerIndex;
            }

            // Handle number answers (1, 2, 3, 4)
            if (int.TryParse(lowerAnswer, out int selectedNumber))
            {
                return (selectedNumber - 1) == CorrectAnswerIndex;
            }

            // Handle true/false
            if (QuestionType == "TrueFalse")
            {
                if (lowerAnswer == "true" || lowerAnswer == "t")
                    return CorrectAnswerIndex == 0;
                if (lowerAnswer == "false" || lowerAnswer == "f")
                    return CorrectAnswerIndex == 1;
            }

            // Handle full text answers
            foreach (string option in Options)
            {
                if (lowerAnswer.Contains(option.ToLower()))
                {
                    return Options.IndexOf(option) == CorrectAnswerIndex;
                }
            }

            return false;
        }

        public string GetCorrectAnswerText()
        {
            if (QuestionType == "TrueFalse")
            {
                return CorrectAnswerIndex == 0 ? "True" : "False";
            }
            else
            {
                return Options[CorrectAnswerIndex];
            }
        }

        public string GetExplanationWithCorrectAnswer()
        {
            return $"Correct answer: {GetCorrectAnswerText()}\n\n{Explanation}";
        }
    }
}