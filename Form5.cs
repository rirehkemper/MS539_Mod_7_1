using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        private string currentUsername; // Store the current user's username
        private List<User> users; // Store all users from the JSON file

        // Constructor accepting the username
        public Form5(string username)
        {
            InitializeComponent();
            currentUsername = username; // Set the username for this session
        }

        // Event triggered when Form5 loads
        private void Form5_Load(object sender, EventArgs e)
        {
            LoadUserProfile(); // Load user's profile data on form load
        }

        // Method to load the user profile from the JSON file
        private void LoadUserProfile()
        {
            string filePath = "userData.json"; // Path to the JSON file in the bin/Debug directory

            if (File.Exists(filePath)) // Check if the file exists
            {
                // Read all users from the JSON file
                string jsonString = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();

                // Find the user profile with the matching username
                User userProfile = users.FirstOrDefault(u => u.Username == currentUsername);

                if (userProfile != null)
                {
                    // Populate the form fields with user profile data
                    textBox5.Text = userProfile.Username;
                    textBox8.Text = userProfile.Password;
                    textBox1.Text = userProfile.FirstName;
                    textBox2.Text = userProfile.MiddleName;
                    textBox3.Text = userProfile.LastName;
                    maskedTextBox1.Text = userProfile.DOB;
                    maskedTextBox2.Text = userProfile.Email;
                    maskedTextBox3.Text = userProfile.Phone;
                    comboBox1.SelectedItem = userProfile.Sex;

                    // Populate the conditions listBox with user's medical conditions
                    listBox1.Items.Clear();
                    foreach (var condition in userProfile.MedicalConditions)
                    {
                        listBox1.Items.Add(condition);
                    }
                }
                else
                {
                    MessageBox.Show("User profile not found.");
                }
            }
            else
            {
                MessageBox.Show("User data file not found.");
            }
        }

        // Method to update the user profile and save to the JSON file
        private void UpdateUserProfile()
        {
            // Find the user in the list and update their details
            User userProfile = users.FirstOrDefault(u => u.Username == currentUsername);
            if (userProfile != null)
            {
                userProfile.FirstName = textBox1.Text;
                userProfile.MiddleName = textBox2.Text;
                userProfile.LastName = textBox3.Text;
                userProfile.DOB = maskedTextBox1.Text;
                userProfile.Email = maskedTextBox2.Text;
                userProfile.Phone = maskedTextBox3.Text;
                userProfile.Password = textBox8.Text;
                userProfile.Sex = comboBox1.SelectedItem?.ToString();

                // Update medical conditions from listBox1
                userProfile.MedicalConditions = listBox1.Items.Cast<string>().ToList();

                // Serialize and save the updated list of users back to JSON
                string filePath = "userData.json";
                string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);

                MessageBox.Show("Profile updated successfully.");
            }
            else
            {
                MessageBox.Show("Could not update profile. User not found.");
            }
        }

        // Event handler for the Update button
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateUserProfile(); // Save the updated profile data
        }

        // User class to match the JSON structure
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string DOB { get; set; }
            public string Username { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Sex { get; set; }
            public List<string> MedicalConditions { get; set; }
        }

        // Event handlers for your controls
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox8_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }
        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            // Gather the medical conditions from the listBox
            List<string> medicalConditions = listBox1.Items.Cast<string>().ToList();

            // Create an instance of Form1, passing both username and medical conditions
            Form1 form1 = new Form1(currentUsername, medicalConditions);
            form1.Show();
            this.Close();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            string condition = textBox9.Text;

            if (!string.IsNullOrEmpty(condition))
            {
                listBox1.Items.Add(condition);
                textBox9.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a medical condition.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string updatedCondition = textBox10.Text;

                if (!string.IsNullOrEmpty(updatedCondition))
                {
                    listBox1.Items[listBox1.SelectedIndex] = updatedCondition;
                    textBox10.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a new value for the selected condition.");
                }
            }
            else
            {
                MessageBox.Show("Please select a condition to update.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
             if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Please select a condition to delete.");
            }
        }
    }
}
