﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PasswordManagerLibrary
{
    public class User
    {
        private const int MINIMUM_PASSWORD_LENGTH = 8;

        private string _username;
        private string _password;

        /// <summary>
        /// Creates a new instance of the User class.
        /// </summary>
        /// <param name="username">A string containing the username</param>
        /// <param name="password">A string containing the password</param>
        /// <exception cref="ArgumentException"></exception>
        public User(string username, string password)
        {
            _set(username, password);
        }
        
        /// <summary>
        /// Sets the username and password of the current instance.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void _set(string username, string password)
        {
            if (_validateUsername(username) && _validatePassword(password))
            {
                _username = username;
                _password = password;
                return;
            }
            throw new ArgumentException(
                    "Username and/or password didn't meet requirements."
                    );
        }

        /// <summary>
        /// Reads and instantiates a user from the input stream.
        /// </summary>
        /// <param name="inputStream">Reference to the input stream.</param>
        internal User(TextReader inputStream)
        {
            string username = inputStream.ReadLine() ?? string.Empty;
            string password = inputStream.ReadLine() ?? string.Empty;
            _set(username, password);
        }

        public string GetUsername()
        {
            return _username;
        }
        public string GetPassword()
        {
            return _password;
        }

        private static bool _validateUsername(string username)
        {
            // Username requirements:

            // Username cannot be empty.
            // Should contain only letters, numbers, and underscore.

            return username.Length > 0 
                && username.All(
                    ch => char.IsLetter(ch) || char.IsDigit(ch) || ch == '_'
                    );
        }
        private static bool _validatePassword(string password)
        {
            // Password requirements:

            // One or more lowecase letters.
            // One or more uppercase letters.
            // One or more digits.
            // At least 8 characters.

            return password.Length >= MINIMUM_PASSWORD_LENGTH
                && password.Any(ch => char.IsLower(ch)) 
                && password.Any(ch => char.IsUpper(ch))
                && password.Any(ch => char.IsDigit(ch));
        }

        /// <summary>
        /// Writes an user to an output stream.
        /// </summary>
        /// <param name="outputStream">Reference to the output stream</param>
        /// <returns>true if the save was successful. Otherwise, returns false.</returns>
        internal bool Save(TextWriter outputStream)
        {
            try
            {
                outputStream.WriteLine(GetUsername());
                outputStream.WriteLine(GetPassword());
            } catch
            {
                return false;
            }
            return true;
        }
    }
}
