using System.Collections;

namespace cybersecurity_chatbot_p2
{
    public class respond
    {
        public respond(ArrayList reply, ArrayList ignore)
        {
            LoadAnswers(reply);
            LoadIgnoreWords(ignore);
        }

        private void LoadIgnoreWords(ArrayList ignoreList)
        {
            string[] ignoreWords = {
                "a", "about", "above", "across", "after", "again", "all", "am", "an", "and",
                "any", "are", "as", "at", "be", "because", "been", "before", "being", "between",
                "both", "but", "by", "can", "could", "did", "do", "does", "doing", "done",
                "during", "each", "either", "else", "for", "from", "get", "give", "had",
                "has", "have", "having", "he", "her", "here", "him", "his", "how", "i",
                "if", "in", "into", "is", "it", "its", "me", "more", "most", "much",
                "my", "never", "no", "nor", "not", "nothing", "of", "off", "on", "once",
                "one", "only", "or", "other", "ought", "our", "out", "over", "own", "same",
                "she", "should", "so", "some", "such", "than", "that", "the", "their", "them",
                "then", "there", "these", "they", "this", "those", "through", "to", "too",
                "under", "until", "up", "very", "was", "we", "were", "what", "when", "where",
                "which", "while", "who", "whom", "why", "will", "with", "would", "you", "your"
            };

            foreach (string word in ignoreWords)
                ignoreList.Add(word);
        }

        private void LoadAnswers(ArrayList answers)
        {
            // PASSWORD TOPIC
            answers.Add("password Use strong passwords with at least 12 characters including uppercase, lowercase, numbers, and symbols.");
            answers.Add("password Never reuse passwords across different accounts. If one gets hacked, all become vulnerable.");
            answers.Add("password Use a password manager to generate and store unique passwords for each account.");
            answers.Add("password Avoid using personal information like birthdays, names, or pet names in your passwords.");
            answers.Add("password Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security.");

            // SCAM TOPIC
            answers.Add("scam Be cautious of unsolicited messages, emails, or calls. Scammers often create a sense of urgency.");
            answers.Add("scam Never share personal information or send money to someone you haven't verified in person.");
            answers.Add("scam If something sounds too good to be true, it probably is a scam. Trust your instincts.");
            answers.Add("scam Legitimate companies will never ask for your password or banking details via email or phone.");

            // PRIVACY TOPIC
            answers.Add("privacy Review your privacy settings on social media regularly. Limit what personal information you share publicly.");
            answers.Add("privacy Use privacy-focused browsers like DuckDuckGo when possible to avoid being tracked online.");
            answers.Add("privacy Be careful about what you post online. Once something is on the internet, it's difficult to remove.");
            answers.Add("privacy Check app permissions on your phone and computer. Only give access to information that is necessary.");
            answers.Add("privacy Use a VPN when on public Wi-Fi to encrypt your internet traffic and protect your personal information.");

            // PHISHING TOPIC
            answers.Add("phishing Phishing attacks try to trick you into revealing sensitive information through fake emails or websites.");
            answers.Add("phishing Always check the URL before entering login credentials. Look for HTTPS and correct domain names.");
            answers.Add("phishing Hover over links before clicking to see the actual destination URL.");
            answers.Add("phishing Be wary of urgent language like 'Your account will be closed' - this creates false urgency.");

            // GREETINGS
            answers.Add("hello Hello! How can I help you with cybersecurity today?");
            answers.Add("hello Hi there! Ready to learn about online safety?");
            answers.Add("hello Greetings! Ask me anything about staying secure online.");
            answers.Add("hi Hello! How can I help you with cybersecurity today?");
            answers.Add("hey Hi there! Ready to learn about online safety?");

            // HELP
            answers.Add("help I can help you with: Passwords, Scams, Privacy, Phishing. Just ask!");
            answers.Add("help Try asking: 'Tell me about password safety' or 'How to avoid scams?'");
            answers.Add("help I'm your cybersecurity assistant - ask me about any online safety topic!");

            // THANKS
            answers.Add("thanks You're welcome! Stay safe online!");
            answers.Add("thanks Glad I could help! Remember to practice good cybersecurity habits.");
            answers.Add("thanks Anytime! Feel free to ask more questions.");

            // SENTIMENT RESPONSES
            answers.Add("worried I understand you're worried. Cybersecurity concerns are normal. Let me help you stay safe.");
            answers.Add("worried Don't worry - most security issues can be resolved. What specific concern do you have?");
            answers.Add("worried It's okay to feel concerned. Let's go through this step by step together.");

            answers.Add("frustrated I hear your frustration. Let's work through this issue together calmly.");
            answers.Add("frustrated Technology can be frustrating. Take a deep breath - I'm here to help solve this.");
            answers.Add("frustrated I understand it's frustrating. Let me explain it in a simpler way.");

            answers.Add("confused That's okay, confusion is normal. I'll explain it clearly for you.");
            answers.Add("confused Let me break it down step by step so it makes sense.");
            answers.Add("confused No worries, I'll help you understand it better.");

            answers.Add("happy That's great to hear! I'm glad things are going well.");
            answers.Add("happy Awesome! Positivity is always good for learning cybersecurity.");
            answers.Add("happy I'm happy for you! Let me know if you need anything.");

            answers.Add("sad I'm sorry you're feeling this way. I'm here for you.");
            answers.Add("sad That sounds tough, take things one step at a time.");
            answers.Add("sad I hope things improve soon. You can talk to me anytime.");

            answers.Add("curious Great curiosity! Cybersecurity is fascinating. What would you like to learn about?");
            answers.Add("curious I love your interest in staying safe online! Let me share some tips.");
            answers.Add("curious Curiosity is the first step to good cybersecurity. Ask me anything!");
        }
    }
}