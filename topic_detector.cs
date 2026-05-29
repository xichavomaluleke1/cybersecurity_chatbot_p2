namespace cybersecurity_chatbot_p2
{
    public class topic_detector
    {
        private string[] topics;

        public topic_detector()
        {
            topics = new string[]
            {
                "password", "scam", "privacy", "phishing", "malware",
                "2fa", "vpn", "firewall", "hacked", "fraud", "cybersecurity"
            };
        }

        public string DetectTopic(string input)
        {
            string lowerInput = input.ToLower();

            foreach (string topic in topics)
            {
                if (lowerInput.Contains(topic))
                {
                    return topic;
                }
            }

            // Check for variations
            if (lowerInput.Contains("pass") || lowerInput.Contains("password"))
                return "password";
            if (lowerInput.Contains("scam") || lowerInput.Contains("fraud"))
                return "scam";
            if (lowerInput.Contains("private") || lowerInput.Contains("privacy"))
                return "privacy";
            if (lowerInput.Contains("phish"))
                return "phishing";

            return "";
        }
    }
}