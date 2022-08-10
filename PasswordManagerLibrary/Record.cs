using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    public class Record : IRecord
    {
        private string _website;
        private string _username;
        private string _password;

        /// <exception cref="InvalidWebsiteException"></exception>
        /// <exception cref="InvalidUsernameException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        private void _set(string website, string username, string password)
        {
            // Set the field values.
            if (!_setWebsite(website))
            {
                throw new InvalidWebsiteException(
                    "A website name cannot be empty, consists of only whitespaces," +
                    " or have newline character(s) in between.");
            }
            if (!SetUsername(username))
            {
                throw new InvalidUsernameException(
                    "A username cannot be empty, consists of only whitespaces, or have" +
                    "whitespaces in between.");
            }
            if (!SetPassword(password))
            {
                throw new InvalidPasswordException(
                    "A password cannot be empty or have newline character(s) in between.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the Record class by reading values from
        /// an open input stream.
        /// </summary>
        /// <param name="inputStream">The reference to an open input stream</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidWebsiteException"></exception>
        /// <exception cref="InvalidUsernameException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        public Record(TextReader inputStream)
        {
            string website;
            string username;
            string password;

            // Read the field values from given input stream.
            try
            {
                website = inputStream.ReadLine() ?? string.Empty;
                username = inputStream.ReadLine() ?? string.Empty;
                password = inputStream.ReadLine() ?? string.Empty;
            } catch
            {
                throw new Exception("Couldn't read from input stream. The stream" +
                    "is closed perhaps?");
            }

            // Set field values.
            _set(website, username, password);
        }

        /// <summary>
        /// Initializes a new instance of the Record class with given website,
        /// username, and password.
        /// </summary>
        /// <exception cref="InvalidWebsiteException"></exception>
        /// <exception cref="InvalidUsernameException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        public Record(string website, string username, string password)
        {
            _set(website, username, password);
        }
        private bool _setWebsite(string website)
        {
            // Leading and trailing whitespaces of a website name are ignored.
            string trimmedWebsite = website.Trim();
            
            // A website name shouldn't contain only whitespaces.
            if (trimmedWebsite.Length == 0) { return false; }

            // A website name should contain any newline character in between.
            if (trimmedWebsite.Any(ch => ch == '\n')) { return false; }

            _website = trimmedWebsite;
            return true;
        }

        public string GetPassword()
        {
            return _password;
        }

        public string GetUsername()
        {
            return _username;
        }

        public string GetWebsite()
        {
            return _website;
        }

        public bool SetPassword(string password)
        {
            // A password cannot be empty. (But it can contain only whitespaces)
            if (password.Length == 0) { return false; }

            // A password should contain any newline character in between.
            if (password.Any(ch => ch == '\n')) { return false; }

            _password = password;
            return true;
        }

        public bool SetUsername(string username)
        {
            // Leading and trailing whitespaces of a username are ignored.
            string trimmedUsername = username.Trim();

            // A username cannot be empty or consists only of whitespaces.
            if (trimmedUsername.Length == 0) { return false; }
            

            // A username cannot contain any whitespace character in between
            // (i.e. besides the leading and the trailing ones).
            if (trimmedUsername.Any(ch => char.IsWhiteSpace(ch)))   // if trimmedUsername
                                                                    // has any whitespace in it
            {
                return false;
            }
            _username = trimmedUsername;
            return true;
        }

        public bool Save(TextWriter outputStream)
        {
            try
            {
                outputStream.WriteLine(GetWebsite());
                outputStream.WriteLine(GetUsername());
                outputStream.WriteLine(GetPassword());
            } catch
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object? obj)
        {
            Record? other = obj as Record;
            if (other != null)
            {
                return GetWebsite() == other.GetWebsite() &&
                    GetUsername() == other.GetUsername() &&
                    GetPassword() == other.GetPassword();
            }
            return false;
        }
    }
}
