# Cybersecurity Awareness Chatbot

A comprehensive WPF-based cybersecurity awareness chatbot application that helps users learn about online safety, manage cybersecurity tasks, test their knowledge through quizzes, and track their activities.

## Project Overview

This application is a complete cybersecurity awareness assistant built with C# and WPF. It features a modern, user-friendly interface with multiple integrated components:

- **Cybersecurity Chatbot** - Interactive assistant for cybersecurity education
- **Task Manager** - Create and manage cybersecurity-related tasks with reminders
- **Cybersecurity Quiz** - Test your knowledge with 15+ questions
- **NLP Engine** - Natural language processing for flexible user interactions
- **Activity Logger** - Track all actions and conversations
- **Sentiment Detection** - Detect user sentiment and respond empathetically

---

## Table of Contents

1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Project Structure](#project-structure)
4. [Setup Instructions](#setup-instructions)
5. [Database Setup](#database-setup)
6. [How to Run](#how-to-run)
7. [Usage Guide](#usage-guide)
8. [Class Descriptions](#class-descriptions)
9. [Screenshots](#screenshots)
10. [Contributors](#contributors)
11. [License](#license)

---

## Features

### Task 1: Task Assistant with Reminders
- Add cybersecurity tasks with titles and descriptions
- Set reminders with specific dates
- View all tasks (pending and completed)
- Mark tasks as completed
- Delete tasks
- Database storage using SQL Server

### Task 2: Cybersecurity Mini-Game (Quiz)
- 15+ cybersecurity questions
- Mix of Multiple Choice and True/False questions
- Immediate feedback with explanations
- Score tracking
- Final score with motivational feedback
- Topics: Password Safety, Phishing, Safe Browsing, Social Engineering, Malware, 2FA, Privacy

### Task 3: Natural Language Processing (NLP) Simulation
- Keyword detection for flexible commands
- Variations in user input recognized
- Task creation: "add task", "create task", "new task"
- Reminder setting: "remind me", "set reminder", "remember to"
- Quiz starting: "start quiz", "take quiz", "quiz me"
- Log viewing: "show log", "activity log", "what have you done"
- Date extraction: "tomorrow", "in 3 days", "next week"

### Task 4: Activity Log Feature
- Logs all significant actions with timestamps
- Actions logged: Task additions, completions, deletions
- Reminder settings
- Quiz starts and completions
- Sentiment detection results
- Topic discussions
- User logins
- View last 10-20 activities
- Filter by action type (Task, Quiz, Reminder, NLP)
- Clear log functionality

---

## Technologies Used

| Technology | Purpose |
|------------|---------|
| **C#** | Main programming language |
| **WPF/XAML** | GUI framework |
| **SQL Server (LocalDB)** | Database for task storage |
| **.NET Framework 4.7.2+** | Application framework |
| **System.Speech** | Voice functionality |
| **System.Data.SqlClient** | Database connectivity |

---

## Project Structure
cybersecurity_chatbot_p2/
├── Models/
│ ├── TaskModel.cs
│ ├── QuizQuestion.cs
│ └── ActivityLogEntry.cs
├── database_helper.cs
├── task_manager.cs
├── quiz_manager.cs
├── nlp_processor.cs
├── activity_logger.cs
├── response_finder.cs
├── response_handler.cs
├── topic_detector.cs
├── sentiment_detector.cs
├── respond.cs
├── voice_greeting.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── App.xaml
└── App.xaml.cs


---

## Setup Instructions

### Prerequisites

1. **Visual Studio 2019 or 2022** (Community edition is fine)
2. **.NET Framework 4.7.2 or higher**
3. **SQL Server LocalDB** (comes with Visual Studio)
4. **Windows 10 or 11** (for voice features)

Chat Commands
Command	Description
add task [task name]	Add a new cybersecurity task
show tasks	View all your tasks
complete task [task name]	Mark a task as completed
delete task [task name]	Delete a task
start quiz	Start the cybersecurity quiz
show log	View recent activity log
show full log	View complete activity log
show task log	View only task activities
show quiz log	View only quiz activities
show reminder log	View only reminder activities
show nlp log	View only NLP activities
help	Show available commands
Task Management
Add a Task

Type: add task Enable 2FA

Or use the "Add New Task" section on the right panel

Set a Reminder

After adding a task, respond: yes, remind me in 3 days

Or type: remind me to update password tomorrow

Complete a Task

Select a task from the list

Click the "Complete" button

Or type: complete task [task name]

Delete a Task

Select a task from the list

Click the "Delete" button

Or type: delete task [task name]

Quiz
Start the Quiz

Click the "Quiz" tab

Click "Start Quiz"

Or type: start quiz

Answer Questions

Type: A, B, C, or D for multiple choice

Type: True or False for true/false questions

View Results

After completing all questions, view your score

Receive motivational feedback

Activity Log
View Log

Click the "Log" tab

Or type: show log

Filter Log

Type: show task log (shows only task activities)

Type: show quiz log (shows only quiz activities)

Clear Log

Click the "Clear" button on the Log tab

Class Descriptions
Class	Purpose
database_helper	Handles all database operations (CRUD)
task_manager	Manages task logic and NLP processing for tasks
quiz_manager	Manages quiz questions, scoring, and feedback
nlp_processor	Processes natural language input and detects intent
activity_logger	Logs all activities with timestamps
response_finder	Finds responses based on topics
response_handler	Handles default responses and follow-up detection
topic_detector	Detects cybersecurity topics in user input
sentiment_detector	Detects user sentiment (happy, worried, frustrated, curious)
respond	Initializes response collections
voice_greeting	Plays audio greeting on startup
TaskModel	Represents a task object
QuizQuestion	Represents a quiz question
ActivityLogEntry	Represents an activity log entry

YouTube Video Link: https://youtu.be/FjjXvM_E98s
