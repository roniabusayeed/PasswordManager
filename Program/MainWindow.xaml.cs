using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using PasswordManagerLibrary;
using System.IO;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Record manager for the current user.
        private readonly RecordManager _recordManager;

        public MainWindow(string username)
        {
            InitializeComponent();

            // Set title.
            this.Title = $"{Constants.APPLICATION_NAME} [{username}]";

            // Initialize the record manager for given username.
            _recordManager = new RecordManager(username);

            websiteTextBox.Focus();
        }

        // Private helper methods.
        private void _search()
        {
            // Get hold of the website.
            string website = websiteTextBox.Text;

            // Search for the website record in the record manager.
            IRecord? record =  _recordManager.FindRecord(website);
            if (record != null)
            {
                // Populate the uesrname and password fields.
                usernameTextBox.Text = record.GetUsername();
                passwordBox.Password = record.GetPassword();

                // Automatically copy the password to clipboard.
                Clipboard.SetText(record.GetPassword());
            } else
            {
                MessageBox.Show($"No record found for website \"{website}\"", Title);
            }
        }

        /// <summary>
        /// Clears all entires.
        /// </summary>
        private void _clear()
        {
            websiteTextBox.Clear();
            usernameTextBox.Clear();
            passwordBox.Clear();
        }

        /// <summary>
        /// Saves/updates the current record from input fields.
        /// </summary>
        private void _save()
        {
            try
            {
                // Create a Record instance from input fields.
                IRecord record = new Record(
                    websiteTextBox.Text,
                    usernameTextBox.Text,
                    passwordBox.Password);

                // Save record.
                if (_recordManager.AddRecord(record))
                {
                    MessageBox.Show($"Record saved successfully for website \"{record.GetWebsite()}\"",
                       Constants.APPLICATION_NAME);
                } else
                {
                    MessageBox.Show($"Failed to save record for website \"{record.GetWebsite()}\"",
                        Constants.APPLICATION_NAME);
                }

            } catch (Exception e)
            {
                MessageBox.Show(
                    $"Error: {e.Message}",
                    Constants.APPLICATION_NAME);
            }
        }

        // Event handlers.
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            _search();
        }

        private void usernameCopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(usernameTextBox.Text);
        }

        private void passwordCopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(passwordBox.Password);
        }
        
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            _clear();
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            _save();
            _clear();
        }

        private void websiteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (! e.IsRepeat)   // So, that KeyDown behaves like KeyPress.
            {
                // Check if Enter key is pressed while websiteTextBox is in focus.
                if (e.Key == Key.Enter)
                {
                    // Do what clicking the search button would do.
                    searchButton_Click(sender, e);
                }
            }
        }

        private void generatePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = Utils.GeneratePassword();
        }
    }
}
