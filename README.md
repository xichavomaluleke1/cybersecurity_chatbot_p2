🔒 cybersecurity_chatbot_p2 - Jordan AI
Project Overview
Jordan AI is a cybersecurity awareness chatbot developed for South African citizens as part of a national cybersecurity education campaign. This Part 2 version transforms the original console application into a modern WPF GUI application with enhanced features including keyword recognition, random responses, conversation flow, memory recall, and sentiment detection.

Features
Part 2 Features:
Feature	Description
🎨 Modern GUI	Clean WPF interface with professional color scheme (dark blue, green, light gray)
🎵 Voice Greeting	Plays a recorded WAV audio greeting when the application starts
👤 User Memory	Remembers returning users via text file storage (users.txt)
🧠 Interest Memory	Saves user interests to user_interests.txt for personalized conversations
🔍 Keyword Recognition	Detects "password", "scam", "privacy", "phishing" topics
🎲 Random Responses	5+ responses per topic with random selection using ArrayList
💬 Conversation Flow	Handles follow-up questions like "another tip", "tell me more", "explain more"
😊 Sentiment Detection	Responds to emotions (worried, frustrated, confused, happy, sad, curious)
⚠️ Error Handling	Graceful handling of empty inputs and unrecognized questions
⌨️ Keyboard Shortcut	Press Enter key to send messages quickly
📁 Code Optimization	Separate classes for each responsibility (OOP principles)
Technical Requirements Met
Requirement	Status
✅ WPF GUI Application	Complete
✅ Voice greeting using System.Media	Complete
✅ Logo image display	Complete
✅ User name collection with file memory	Complete
✅ Keyword recognition (password, scam, privacy)	Complete
✅ Random responses (3+ per topic)	Complete
✅ Conversation flow (follow-up questions)	Complete
✅ Memory and recall (user name + interests)	Complete
✅ Sentiment detection (worried, frustrated, confused, happy, sad, curious)	Complete
✅ Error handling for empty inputs	Complete
✅ Colored chat messages	Complete
✅ Object-oriented design with multiple classes	Complete
✅ Generic collection (ArrayList)	Complete
✅ Delegates (event handlers)	Complete
Project Structure
text
cybersecurity_chatbot_p2/
│
├── MainWindow.xaml              # GUI layout (Home, Username, Chat grids)
├── MainWindow.xaml.cs           # Main logic and event handlers
│
├── respond.cs                   # All responses and ignore words storage
├── voice_greeting.cs            # Voice greeting playback
├── response_finder.cs           # Finds responses for specific topics
├── response_handler.cs          # Follow-up and default response handling
├── topic_detector.cs            # Detects cybersecurity topics
├── sentiment_detector.cs        # Detects user sentiment
├── message_displayer.cs         # Formats and displays chat messages
│
├── voice_recording.wav          # Voice greeting audio file
├── logo.jpeg                    # Logo image for GUI
│
├── users.txt                    # Auto-generated - stores usernames
└── user_interests.txt           # Auto-generated - stores user interests
Class Descriptions
MainWindow.xaml.cs
Main application logic and event handlers

Handles UI navigation between grids (home, username, chat)

Manages the send button and Enter key response processing

Coordinates all other classes

Processes user input and displays responses

voice_greeting.cs
Plays a WAV audio greeting when the application launches

Uses System.Media.SoundPlayer class

Auto-detects the correct file path

Silently fails if sound file not found

respond.cs
Stores all response data using ArrayList

Topics include: password, scam, privacy, phishing, greeting, help, thanks

Stores the ignore words list (stop words like "a", "an", "the")

Contains 5+ password responses, 5 scam responses, 5 privacy responses

Contains sentiment responses for 6+ emotions

response_finder.cs
Finds random responses for specific cybersecurity topics

Searches the reply ArrayList for responses starting with a topic

Returns a randomly selected response

Handles related topic variations (e.g., "pass" → "password")

response_handler.cs
Handles follow-up question detection

Provides default responses for unrecognized input

Manages ignore word checking

topic_detector.cs
Detects cybersecurity topics from user input

Recognizes keywords for password, scam, privacy, phishing

Returns the topic name as a string

Handles topic variations automatically

sentiment_detector.cs
Detects user emotions from input text

Recognizes: worried, frustrated, confused, happy, sad, curious

Provides empathetic responses based on detected sentiment

Returns cybersecurity tips tailored to user's mood

message_displayer.cs
Formats and displays chat messages with colors

Shows user messages in light green (right-aligned)

Shows bot messages in light blue (left-aligned)

Handles scrolling to latest message

Key Methods
MainWindow.xaml.cs
Method	Description
proceed()	Navigates from home to username grid
submit_name()	Saves username and shows chat interface
send()	Processes user input and triggers response
ProcessUserInput()	Main logic for sentiment, follow-ups, and topics
AddMessage()	Displays formatted messages in chat
SaveUserToFile()	Stores username for memory recall
RecallUserInterests()	Loads previous interests from file
sentiment_detector.cs
Method	Description
DetectSentiment()	Detects user emotion from input text
GetSentimentResponse()	Returns empathetic response for detected emotion
response_finder.cs
Method	Description
GetResponseByTopic()	Returns random response for specific topic
GetResponsesByKeyword()	Returns all responses matching a keyword
topic_detector.cs
Method	Description
DetectTopic()	Identifies cybersecurity topic from user input
message_displayer.cs
Method	Description
AddMessage()	Displays user or bot message in chat
Prerequisites
Requirement	Version
Windows Operating System	10 or later
.NET Framework	4.7.2 or later
Visual Studio	2019 or 2022
Audio Output	For voice greeting feature
Installation & Setup
Step 1: Clone the Repository
bash
git clone https://github.com/yourusername/cybersecurity_chatbot_p2.git
Step 2: Open the Project
Navigate to the project folder

Double-click cybersecurity_chatbot_p2.sln

Open with Visual Studio

Step 3: Add Required Assets
Add logo.jpeg to the project root folder

Add greet.wav to the project root folder

Step 4: Build and Run
Press F5 or click Debug > Start Debugging

Usage Guide
Starting the Application
Home Screen - Click "Start Chatting"

Username Entry - Enter your name (returning users get a welcome back message)

Chat Interface - Start asking cybersecurity questions

Example Questions to Ask
General Questions
Question
"How are you?"
"What's your purpose?"
"What can I ask you about?"
"help"
Password Safety
Question
"Tell me about passwords"
"How do I create a strong password?"
"Password safety tips"
"What makes a good password?"
Scam Detection
Question
"What is a scam?"
"How to avoid online scams?"
"Tell me about fraud"
"How to spot a scammer?"
Privacy Protection
Question
"How can I protect my privacy?"
"Tell me about privacy"
"Privacy protection tips"
"How to stay private online?"
Phishing Awareness
Question
"What is phishing?"
"How to spot a phishing email?"
"Tell me about phishing attacks"
Follow-up Questions
Question
"Another tip"
"Tell me more"
"Explain more"
"More information"
"Elaborate"
Sentiment Expressions
Emotion	Example
Worried	"I'm worried about online scams"
Frustrated	"This is frustrating"
Confused	"I'm confused about cybersecurity"
Happy	"I'm happy with that tip"
Sad	"That makes me sad"
Curious	"I'm curious about hacking"
Expressing Interests
Example
"I'm interested in privacy"
"I like learning about passwords"
Example Conversation Flow
text
Home Screen → Start Chatting → Username: Pande

jordan: Hey Pande welcome back, how can i help you today

Pande: tell me about passwords

jordan: Use strong passwords with at least 12 characters including uppercase, 
        lowercase, numbers, and symbols.

Pande: another tip

jordan: Here's more about password:
        Never reuse passwords across different accounts.

Pande: I'm interested in privacy

jordan: Great! I'll remember that you're interested in privacy.

Pande: I'm worried about hackers

jordan: I understand you're worried. Cybersecurity concerns are normal. 
        Let me help you stay safe.
Video Presentation
An unlisted YouTube video presentation demonstrating:

Full application functionality

Code structure explanation

Logic and flow demonstration

Voice greeting playback

Keyword recognition

Random responses

Conversation flow (follow-up questions)

Sentiment detection

Memory and recall features

YouTube Video link: [Insert your video link here]

GitHub Information
Item	Details
Repository Name	cybersecurity_chatbot_p2
Default Branch	master
Commits	6+ commits
CI/CD	GitHub Actions workflow
Releases	v1.0, v2.0
Issues	None open
Author
Pandelani Vhatwenga

Detail	Information
Course	DISD0601 Y2
Part	2 of 3
Institution	ROSEBANK INTERNATIONAL COLLEGE
Project	Cybersecurity Awareness Chatbot - Jordan AI
