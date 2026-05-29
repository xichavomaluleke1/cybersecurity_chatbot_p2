using System;
using System.Collections;

namespace cybersecurity_chatbot_p2
{//start of namespace
    public class response_finder
    {//start of class
        private ArrayList replies;
        private Random random;

        public response_finder(ArrayList replies)
        {//start of constructor
            this.replies = replies;
            this.random = new Random();
        }// end of constructor

        public string GetResponseByTopic(string topic)
        {
            string lowerTopic = topic.ToLower();
            ArrayList matchingResponses = new ArrayList();
            //looking for responses that start with the topic followed by a space
            foreach (string response in replies)
            {// start of foreach
                string responseStr = response.ToString().ToLower();
                if (responseStr.StartsWith(lowerTopic))
                {//start of inner if statement
                    matchingResponses.Add(response);
                }//end of innerif statement
            }

            if (matchingResponses.Count > 0)
            {
                string response = matchingResponses[random.Next(matchingResponses.Count)].ToString();
                int spaceIndex = response.IndexOf(' ');
                if (spaceIndex > 0)
                    return response.Substring(spaceIndex + 1);
                return response;
            }

            return "I'm here to help with cybersecurity topics like passwords, scams, and privacy.";
        }

        public ArrayList GetResponsesByKeyword(string keyword)
        {
            ArrayList results = new ArrayList();
            string lowerKeyword = keyword.ToLower();

            foreach (string response in replies)
            {
                if (response.ToString().ToLower().StartsWith(lowerKeyword))
                {
                    results.Add(response);
                }
            }

            return results;
        }//end of method
    }//end of class
}//end ofnamespace