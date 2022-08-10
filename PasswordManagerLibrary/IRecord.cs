using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    public interface IRecord
    {
        /// <summary>
        /// Gets the website field of the record.
        /// </summary>
        /// <returns>A string containing the name of the website.</returns>
        string GetWebsite();

        /// <summary>
        /// Gets the username field of the record.
        /// </summary>
        /// <returns>A string containing the username</returns>
        string GetUsername();

        /// <summary>
        /// Gets the password field of the record.
        /// </summary>
        /// <returns>A string containing the password.</returns>
        string GetPassword();

        /// <summary>
        /// Sets the username field of the record.
        /// </summary>
        /// <param name="username">A string containing the username to be set.</param>
        /// <returns>true if the username is set successfully. Otherwise, the username
        /// remains unchanged and it returns false.</returns>
        bool SetUsername(string username);
        
        /// <summary>
        /// Sets the password field of the record.
        /// </summary>
        /// <param name="password">A string containing the password to be set.</param>
        /// <returns>true if the password is set successfully. Otherwise, the password
        /// remains unchanged and it returns false.</returns>
        bool SetPassword(string password);
    }
}
