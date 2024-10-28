using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.ComponentModel.Design.Serialization;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //created variables for username and medical conditions to be loaded on the form initialization
        private string username;
        private List<string> medicalConditions;

        // Single constructor to initialize Form1 with user and medical conditions
        public Form1(string user, List<string> conditions)
        {
            InitializeComponent();
            username = user;
            medicalConditions = conditions;
            PopulateMedicalConditions();
            LoadUserEvents();
        }
        //takes the medical conditions from the .JSON file and places on a new line in textBox3
        private void PopulateMedicalConditions()
        {
            textBox3.Text = string.Join(Environment.NewLine, medicalConditions);
        }

        //returns the user to the login page
        private void button2_Click(object sender, EventArgs e)
        {
            var form2 = new Form2(medicalConditions);
            form2.Show();
            this.Close();
        }
        //allows the user to schedule an event
        private void button3_Click(object sender, EventArgs e)
        {
            var form4 = new Form4(username);
            form4.ShowDialog();
        }

        //takes the events from the event calendar and puts them on the user's "dashboard"
        private void LoadUserEvents()
        {
            string filePath = $"{username}_events.json";

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                var userEvents = JsonSerializer.Deserialize<List<Event>>(jsonString) ?? new List<Event>();

                textBox1.Clear();
                foreach (var evt in userEvents)
                {
                    textBox1.AppendText($"{evt.EventDate.ToString("d")} - {evt.EventName}: {evt.Description}\r\n");
                }
            }
            else
            {
                textBox1.Text = "No scheduled events found.";
            }
        }
        //creates and event
        public class Event
        {
            public DateTime EventDate { get; set; }
            public string EventName { get; set; }
            public string Description { get; set; }
        }
        //takes user to edit profile page
        private void button4_Click(object sender, EventArgs e)
        {
            var form5 = new Form5(username);
            form5.Show();
            this.Close();
        }
        //loads the user's data to the "dashboard"
        public void LoadUserData()
        {
            if (File.Exists("userData.json"))
            {
                try
                {
                    string jsonData = File.ReadAllText("userData.json");
                    var users = JsonSerializer.Deserialize<List<User>>(jsonData); // Deserialize to a List of Users

                    if (users != null)
                    {
                        // Find the user with the matching username
                        var user = users.Find(u => u.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true);

                        if (user != null)
                        {
                            label3.Text = $"Welcome, {user.Username}"; // Display "Welcome" message with the first name
                            textBox3.Text = string.Join(Environment.NewLine, user.MedicalConditions);
                        }
                        else
                        {
                            MessageBox.Show("User data not found.");
                        }
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error reading user data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("User data file not found.");
            }
        }


        //getters and setters connecting user's data making available to other methods and/or functions
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


        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
