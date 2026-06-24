--creating a database called chatbot_tasks
create database chatbot_tasks;

-- use the chatbot_tasks database
use [chatbot_tasks];

-- creating a table called tasks
create table tasks(
task_id int primary key identity(1,1),
task_name varchar(100),
task_description varchar(100),
task_due_date varchar(20),
task_status varchar(20)
);

--select all from the table tasks
select * from tasks;