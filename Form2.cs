﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        // Transferring the medical list from Form3 to this form
        private List<string> medCon;

        public Form2(List<string> conditions)
        {
            InitializeComponent();
            medCon = conditions ?? new List<string>();  // Ensure medCon is not null

            // Attach the KeyDown event for the password field
            textBox2.KeyDown += textBox2_KeyDown;
        }
        private bool isFormSubmitted = false;
        private void button1_Click(object sender, EventArgs e)
        {
            //I was getting multiple instances of my form opening, created logic to prevent that
            //I added the variable as a boolean set to false, and added the logic here and in the 
            //Keydown logic
            if (!isFormSubmitted)
            {
                SubmitForm();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !isFormSubmitted)
            {
                e.SuppressKeyPress = true;
                SubmitForm();
            }
        }

        private void SubmitForm()
        {
            //I set the flag at the beginning of the submission form to prevent multiple instances of Form1
            isFormSubmitted = true;
            string filePath = "userData.json";
            List<User> users;

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("No registered users found. Please register first.");
                isFormSubmitted = false;
                return;
            }

            try
            {
                // Read user data from the JSON file
                string jsonData = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>(); // Handle null case
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading user data: {ex.Message}");
                isFormSubmitted = false;
                return;
            }

            // Get entered username and password
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                isFormSubmitted = false;
                return;
            }

            // Find the user in the user list
            User foundUser = users.FirstOrDefault(x => x.Username == username && VerifyPassword(password, x.Password));

            if (foundUser != null)
            {
                // Open the dashboard (Form1) for the user
                Form1 userDashboard = new Form1(foundUser.Username, foundUser.MedicalConditions);
                userDashboard.Show();
                this.Hide();
            }
            else
            {
                // Login failed
                MessageBox.Show("Invalid username or password.");
                isFormSubmitted = false;
            }
        }

        // User class with getters and setters
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Username { get; set; } // Added Username to match the login logic
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public string Sex { get; set; }
            public List<string> MedicalConditions { get; set; }
            public List<string> Events { get; set; }
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Simple password check (for more security, consider using password hashing)
            return enteredPassword == storedPassword;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Show password
                textBox2.PasswordChar = '\0';
            }
            else
            {
                // Hide password
                textBox2.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
