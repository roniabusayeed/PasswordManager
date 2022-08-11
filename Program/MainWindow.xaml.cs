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
        // Name of the file to save records to or laod records from.
        private const string FILENAME = "saveFile.txt";
        private readonly RecordManager _recordManager;

        public MainWindow()
        {
            InitializeComponent();

            // Load saved records (if any).
            TextReader? inputStream = null;
            try
            {
                inputStream = new StreamReader(FILENAME);
                _recordManager = RecordManager.Load(inputStream) ?? new();
            } catch
            {
                _recordManager = new();
            } finally
            {
                inputStream?.Close();
            }
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
        private void _clear()
        {
            websiteTextBox.Clear();
            usernameTextBox.Clear();
            passwordBox.Clear();
        }

        /// <summary>
        /// Saves/updates the current record from input fields and writes
        /// the changes to file.
        /// </summary>
        private void _save()
        {
            _addToRecordManager();
            _saveToHardDrive();
        }

        /// <summary>
        /// Tries to create a new instance of Record class and adds (or updates)
        /// it to the _recordManager field.
        /// </summary>
        private void _addToRecordManager()
        {
            try
            {
                // Create a Record instance from input fields.
                IRecord record = new Record(
                    websiteTextBox.Text,
                    usernameTextBox.Text,
                    passwordBox.Password);

                // Add the instance to the record manager.
                _recordManager.AddRecord(record);
            } catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}", Title);
            }
        }

        /// <summary>
        /// Saves the current state of _recordManager to file.
        /// </summary>
        private void _saveToHardDrive()
        {
            TextWriter? outputStream = null;
            bool status = false;
            try
            {
                outputStream = new StreamWriter(FILENAME);
                status = _recordManager.Save(outputStream);
            }
            catch
            {
                MessageBox.Show($"Error: Couldn't open file \"{FILENAME}\" for writing.", Title);
            }
            finally
            {
                outputStream?.Close();
            }

            if (!status)
            {
                MessageBox.Show($"Error: Couldn't save to file \"{FILENAME}\"", Title);
            }
        }

        // Event handlers.
        private void websiteTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Check if Enter key is pressed while websiteTextBox is in focus.
            if (e.Key == Key.Enter)
            {
                // Do what clicking the search button would do.
                searchButton_Click(sender, e);
            }
        }

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
    }
}
