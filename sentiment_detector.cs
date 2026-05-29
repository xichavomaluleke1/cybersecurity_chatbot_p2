using System;
using System.Collections;

namespace cybersecurity_chatbot_p2
{
    public class sentiment_detector
    {
        private ArrayList replies;
        private response_finder finder;
        private Random random;

        public sentiment_detector(ArrayList replies, response_finder finder)
        {//start of constructor
            this.replies = replies;
            this.finder = finder;
            this.random = new Random();
        }//end of constructor

        public string DetectSentiment(string input)
        {//start of method
            string lower = input.ToLower();
            //check for confused first(priority)
            if (lower.Contains("worried") || lower.Contains("nervous") || lower.Contains("anxious") || lower.Contains("concerned"))
                return "worried";
            if (lower.Contains("curious") || lower.Contains("interesting") || lower.Contains("want to learn"))
                return "curious";
            if (lower.Contains("frustrated") || lower.Contains("annoyed") || lower.Contains("angry") || lower.Contains("upset"))
                return "frustrated";
            if (lower.Contains("confused"))
                return "confused";
            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good") || lower.Contains("awesome"))
                return "happy";
            if (lower.Contains("sad") || lower.Contains("unhappy"))
                return "sad";

            return "neutral";
        }
        //method to get response for detected sentiment from reply ArrayList
        public string GetSentimentResponse(string sentiment)
        {
            ArrayList responses = finder.GetResponsesByKeyword(sentiment);
            //Find all responses that start with the sentiment
            if (responses.Count > 0)
            {
                string response = responses[random.Next(responses.Count)].ToString();
                int spaceIndex = response.IndexOf(' ');
                if (spaceIndex > 0)
                    return response.Substring(spaceIndex + 1);
                return response;
            }

            // Default responses per sentiment
            switch (sentiment)
            {
                case "worried": return "I understand your concern. Let me help you stay safe online.";
                case "curious": return "Great! What would you like to know about cybersecurity?";
                case "frustrated": return "I hear your frustration. Let's work through this together.";
                case "confused": return "That's okay! Let me explain it more clearly.";
                case "happy": return "I'm glad to hear that! Stay positive and stay safe online!";
                case "sad": return "I'm sorry you're feeling this way. I'm here to help.";
                default: return "I'm here to help you with your cybersecurity questions.";
            }
        }
    }
}