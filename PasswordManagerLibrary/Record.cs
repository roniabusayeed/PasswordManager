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

        /// <summary>
        /// Initializes a new instance of the Record class with given website,
        /// username, and password.
        /// </summary>
        /// <exception cref="InvalidWebsiteException"></exception>
        /// <exception cref="InvalidUsernameException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        public Record(string website, string username, string password)
        {
            if (! _setWebsite(website)) 
            { 
                throw new InvalidWebsiteException(
                    "A website name cannot be empty or consists of only whitespaces.");
            }
            if (! SetUsername(username)) 
            { 
                throw new InvalidUsernameException(
                    "A username cannot be empty, consists of only whitespaces, or have" +
                    "whitespaces in between."); 
            }
            if (! SetPassword(password)) 
            { 
                throw new InvalidPasswordException(
                    "A password cannot be empty."); 
            }
        }
        private bool _setWebsite(string website)
        {
            // Leading and trailing whitespaces of a website name are ignored.
            string trimmedWebsite = website.Trim();
            
            // A website name shouldn't contain only whitespaces.
            if (trimmedWebsite.Length == 0) { return false; }

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
    }
}
