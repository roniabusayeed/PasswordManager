using PasswordManagerLibrary;
using System.Windows;
using System.Windows.Input;

namespace Program
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();

            Title = Constants.APPLICATION_NAME;
            usernameTextBox.Focus();
        }

        // Private helper methods.
        private void _logIn()
        {
            // Get hold of username and passwords.
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            // Authenticate.
            Authenticator authenticator = Authenticator.GetInstance();
            if (authenticator.Authenticate(username, password))
            {
                // Upon successful authentication, go to the main window
                // and close this one.
                var mainWindow = new MainWindow(username);  // passing data to the main window
                                                            // via constructor arguments.
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // Authentication failed. Display message.
                MessageBox.Show(
                    Constants.INVALID_LOGIN_MESSAGE,
                    Constants.APPLICATION_NAME);
            }
        }
        private void _signUp()
        {
            // Get hold of username and passwords.
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            try
            {
                User newUser = new(username, password);
                if (Authenticator.GetInstance().AddUser(newUser))
                {
                    MessageBox.Show(
                        "New profile created for \"" + newUser.GetUsername() + "\"",
                        Constants.APPLICATION_NAME);
                }
                else
                {
                    MessageBox.Show(
                        $"There's already a profile with username \"{newUser.GetUsername()}\"",
                        Constants.APPLICATION_NAME);
                }
            }
            catch
            {
                MessageBox.Show(
                    Constants.SIGN_UP_REQUIREMENTS,
                    Constants.APPLICATION_NAME);
            }
        }

        /// <summary>
        /// Clears all entries.
        /// </summary>
        private void _clear()
        {
            usernameTextBox.Clear();
            passwordBox.Clear();
        }

        // Event handlers.
        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)  // So that KeyDown behaves like KeyPress.
            {
                if (e.Key == Key.Enter)
                {
                    // Enter key was pressed while the passwordBox was in focus.
                    _logIn();
                }
            }
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            _logIn();
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            _signUp();
            _clear();
        }
    }
}
