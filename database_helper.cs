using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using cybersecurity_chatbot_p2.Models;

namespace cybersecurity_chatbot_p2
{
    public class database_helper
    {
        private string connectionString;

        public database_helper()
        {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=chatbot_tasks;Integrated Security=True;";
            EnsureDatabaseAndTableExist();
        }

        private void EnsureDatabaseAndTableExist()
        {
            try
            {
                // First connect to master to create database if needed
                string masterConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

                using (SqlConnection conn = new SqlConnection(masterConnection))
                {
                    conn.Open();

                    // Check if database exists
                    string checkDbQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = 'chatbot_tasks'";
                    using (SqlCommand cmd = new SqlCommand(checkDbQuery, conn))
                    {
                        int dbCount = (int)cmd.ExecuteScalar();
                        if (dbCount == 0)
                        {
                            // Create database
                            string createDbQuery = "CREATE DATABASE chatbot_tasks";
                            using (SqlCommand createCmd = new SqlCommand(createDbQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    conn.Close();
                }

                // Now connect to chatbot_tasks to create table
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM sys.tables WHERE name = 'tasks'";
                    using (SqlCommand cmd = new SqlCommand(checkTableQuery, conn))
                    {
                        int tableCount = (int)cmd.ExecuteScalar();
                        if (tableCount == 0)
                        {
                            // Create table
                            string createTableQuery = @"
                                CREATE TABLE tasks (
                                    task_id INT PRIMARY KEY IDENTITY(1,1),
                                    task_name VARCHAR(100),
                                    task_description VARCHAR(100),
                                    task_due_date VARCHAR(20),
                                    task_status VARCHAR(20)
                                )";

                            using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            // Insert sample data
                            string insertDataQuery = @"
                                INSERT INTO tasks (task_name, task_description, task_status) VALUES
                                ('Enable Two-Factor Authentication', 'Set up 2FA on email and banking accounts', 'Pending'),
                                ('Review Privacy Settings', 'Check and update privacy settings on social media', 'Pending'),
                                ('Update Passwords', 'Change passwords for all online accounts', 'Pending')";

                            using (SqlCommand insertCmd = new SqlCommand(insertDataQuery, conn))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up database: {ex.Message}", "Database Setup", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool InsertTask(TaskModel task)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO tasks 
                                    (task_name, task_description, task_due_date, task_status) 
                                    VALUES 
                                    (@name, @description, @dueDate, @status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", task.TaskName);
                        cmd.Parameters.AddWithValue("@description", (object)task.TaskDescription ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@dueDate", (object)task.TaskDueDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@status", task.TaskStatus);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public List<TaskModel> GetAllTasks()
        {
            List<TaskModel> tasks = new List<TaskModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT * FROM tasks ORDER BY 
                                        CASE WHEN task_status = 'Pending' THEN 0 ELSE 1 END,
                                        task_id DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TaskModel task = new TaskModel
                                {
                                    TaskId = Convert.ToInt32(reader["task_id"]),
                                    TaskName = reader["task_name"].ToString(),
                                    TaskDescription = reader["task_description"]?.ToString(),
                                    TaskDueDate = reader["task_due_date"]?.ToString(),
                                    TaskStatus = reader["task_status"].ToString()
                                };

                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return tasks;
        }

        public bool UpdateTaskStatus(int taskId, string newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE tasks 
                                    SET task_status = @status 
                                    WHERE task_id = @taskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@taskId", taskId);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"DELETE FROM tasks WHERE task_id = @taskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool UpdateTaskDueDate(int taskId, string dueDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE tasks 
                                    SET task_due_date = @dueDate 
                                    WHERE task_id = @taskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dueDate", dueDate);
                        cmd.Parameters.AddWithValue("@taskId", taskId);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public string GetTaskName(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT task_name FROM tasks WHERE task_id = @taskId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Unknown Task";
                    }
                }
            }
            catch
            {
                return "Unknown Task";
            }
        }
    }
}