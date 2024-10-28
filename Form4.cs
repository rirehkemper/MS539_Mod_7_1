using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {   // holds the current user's username
        private string currentUserName; 
        // stores the user's events
        private List<Event> userEvents = new List<Event>(); 
        //constructor brings in the username on initialization
        public Form4(string username) 
        {
            InitializeComponent();
            currentUserName = username; 
            //method for loading the user's events
            LoadUserEvents(); 
        }
        //getters and setters for event
        public class Event
        {
            public DateTime EventDate { get; set; }
            public string EventName { get; set; }
            public string Description { get; set; }
        }
        //method for loading the users events from the .json file
        private void LoadUserEvents()
        {
            string filePath = $"{currentUserName}_events.json";

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the file and deserialize the JSON data to userEvents list
                string jsonString = File.ReadAllText(filePath);
                userEvents = JsonSerializer.Deserialize<List<Event>>(jsonString) ?? new List<Event>();

                // Populate the listBox with previously saved events
                foreach (var evt in userEvents)
                {
                    listBox1.Items.Add($"{evt.EventDate.ToString("d")} - {evt.EventName}");
                }
            }
        }
        //saves the users' events
        private void SaveUserEvents()
        {
            string filePath = $"{currentUserName}_events.json";
            string jsonString = JsonSerializer.Serialize(userEvents);
            File.WriteAllText(filePath, jsonString);
        }
        //puts event name and date into the textbox
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionRange.Start;

            string eventName = Prompt.ShowDialog("Enter Event Name", "Event");
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event name cannot be empty.");
                return; 
            }

            string description = Prompt.ShowDialog("Enter Event Description (optional)", "Event Description");

            Event newEvent = new Event
            {
                EventDate = selectedDate,
                EventName = eventName,
                Description = description
            };

            userEvents.Add(newEvent);
            listBox1.Items.Add($"{selectedDate.ToString("d")} - {eventName}");

            SaveUserEvents();
            MessageBox.Show("Event saved successfully.");
        }


        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox inputBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
         
            LoadUserEvents(); 
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Logic to execute when the date changes
            DateTime selectedDate = e.Start; 
            MessageBox.Show($"You selected: {selectedDate.ToShortDateString()}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the listBox
            if (listBox1.SelectedItem != null)
            {
                // Get the selected event's text (to identify which event to delete)
                string selectedItemText = listBox1.SelectedItem.ToString();

                // Find the matching event in the userEvents list
                var eventToDelete = userEvents.Find(evt =>
                    $"{evt.EventDate.ToString("d")} - {evt.EventName}" == selectedItemText);

                if (eventToDelete != null)
                {
                    // Remove the event from the userEvents list
                    userEvents.Remove(eventToDelete);

                    // Remove the item from the listBox
                    listBox1.Items.Remove(listBox1.SelectedItem);

                    // Save the updated events list to the JSON file
                    SaveUserEvents();

                    MessageBox.Show("Event deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Event not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select an event to delete.");
            }
        }

       
    }
}
