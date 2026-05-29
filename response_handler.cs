using System;
using System.Collections;

namespace cybersecurity_chatbot_p2
{//start of namspace
    internal class response_handler
    {//start of class
        private ArrayList replies;
        private ArrayList ignoreWords;
        private string[] defaultResponses;
        private Random random;

        public response_handler(ArrayList replies, ArrayList ignoreWords)
        {//start of constructor
            this.replies = replies;
            this.ignoreWords = ignoreWords;
            this.random = new Random();

            defaultResponses = new string[]
            {
                "I'm not sure I understand. Can you try rephrasing? Try asking about passwords, scams, or privacy.",
                "Hmm, I didn't quite get that. Would you like to learn about password safety, avoiding scams, or protecting your privacy?",
                "I'm not familiar with that topic. Ask me about cybersecurity like passwords, phishing, or malware!",
                "Could you rephrase that? I specialize in cybersecurity topics like online safety and privacy protection."
            };
        }

        public string GetDefaultResponse()
        {
            return defaultResponses[random.Next(defaultResponses.Length)];
        }

        public bool IsFollowUpRequest(string input)
        {
            string lower = input.ToLower();
            string[] followUpPhrases = {
                "tell me more", "explain more", "more information", "another tip",
                "give me another", "elaborate", "continue", "what else", "more details"
            };

            foreach (string phrase in followUpPhrases)
            {
                if (lower.Contains(phrase))
                    return true;
            }
            return false;
        }

        public bool IsIgnoredWord(string word)
        {
            return ignoreWords.Contains(word.ToLower());
        }//end of method
    }//end of class
}//end of namespace