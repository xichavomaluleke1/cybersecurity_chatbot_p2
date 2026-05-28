using System;
using System.Media;

namespace cybersecurity_chatbot_p2
{//start of namespace
    internal class voice_greeting
    {//start of class
        public voice_greeting()
        {//start of constructor

            //replace the \bin\Debug\ from the path with greeting.wav

            string auto_path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug\", @"\greet.wav");

            //create an instance for the soundPlayer class
            SoundPlayer greetMe = new SoundPlayer(auto_path);
            //then greet
            greetMe.Play();

        }//end of constructor

    }//end of class

}//end of namespacee