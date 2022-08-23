using PasswordManagerLibrary;
using System;
using System.Windows;
using System.Windows.Input;

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
            IRecord? record = _recordManager.FindRecord(website);
            if (record != null)
            {
                // Populate the uesrname and password fields.
                usernameTextBox.Text = record.GetUsername();
                passwordBox.Password = record.GetPassword();

                // Automatically copy the password to clipboard.
                Clipboard.SetText(record.GetPassword());
            }
            else
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

                // If the user is about to modify an existing record,
                // ask for their confirmation.
                IRecord? foundRecord = _recordManager.FindRecord(websiteTextBox.Text);

                // Record already exists for given website.
                if (foundRecord != null)    
                {
                    // And its values have been modified.
                    if (!record.Equals(foundRecord))    
                    {
                        // Ask the user for confirmation.
                        MessageBoxResult messageBoxResult = MessageBox.Show(
                        $"There's an existing record for website \"{websiteTextBox.Text}\"" +
                        $"\n\nAre you sure you want to modify it?",
                        Constants.APPLICATION_NAME,
                        MessageBoxButton.YesNo);

                        if (messageBoxResult == MessageBoxResult.No)
                        {
                            // Don't save the changes.
                            return; 
                        }
                    }

                    // But its values haven't been modified.
                    else
                    {
                        // Nothing to change.
                        return; 
                    }
                }

                // Save record.
                if (_recordManager.AddRecord(record))
                {
                    MessageBox.Show($"Record saved successfully for website \"{record.GetWebsite()}\"",
                       Constants.APPLICATION_NAME);
                }
                else
                {
                    MessageBox.Show($"Failed to save record for website \"{record.GetWebsite()}\"",
                        Constants.APPLICATION_NAME);
                }
            }
            catch (Exception e)
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
        }

        private void websiteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)   // So, that KeyDown behaves like KeyPress.
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

        private void removeRecordButton_Click(object sender, RoutedEventArgs e)
        {
            // Get a hold of the website name.
            string website = websiteTextBox.Text;

            if (_recordManager.FindRecord(website) != null) // Check if the record exists.
            {
                // Ask for user confirmation.
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to remove \"{website}\" record?"
                    , Constants.APPLICATION_NAME, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes) // User confirmed.
                {
                    if (_recordManager.RemoveRecord(website))
                    {
                        MessageBox.Show($"\"{website}\" record removed successfully.");
                        _clear();
                    } else
                    {
                        MessageBox.Show($"\"{website}\" record removal was not successful.");
                    }
                }

            } else  // Record doesn't exist.
            {
                MessageBox.Show("Website not found.", Constants.APPLICATION_NAME);
            }
            
        }
    }
}
