using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    public class Authenticator
    {
        // Implement Authenticator as a Singleton.
        private static Authenticator _instance = new Authenticator();

        /// <summary>
        /// Gets the singleton instance of Authenticator class.
        /// </summary>
        /// <returns>The singleton instance.</returns>
        public static Authenticator GetInstance() { return _instance; }
        
        
        private Dictionary<string, User> _usersDictionary;
        private Authenticator()
        {
            _usersDictionary = new Dictionary<string, User>();
            _loadUsers();   // if any.
        }

        /// <summary>
        /// Loads all users to the users dictionary.
        /// </summary>
        /// <returns>true if the load was successful. Otherwise, returns false.</returns>
        private bool _loadUsers()
        {
            string userDictionaryFilepPath = _getUsersDictionaryFilePath();
            TextReader? inputStream = null;
            try
            {
                inputStream = new StreamReader(userDictionaryFilepPath);
                string usersCountText = inputStream.ReadLine() ?? string.Empty;
                int usersCount = int.Parse(usersCountText);
                for (int i = 0; i < usersCount; i++)
                {
                    User user = new(inputStream);
                    _usersDictionary[user.GetUsername()] = user;
                }
                return true;
            } catch
            {
                return false;
            } finally
            {
                inputStream?.Close();
            }
        }


        /// <summary>
        /// Adds an user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if addinig user was successful. Otherwise, returns false.</returns>
        public bool AddUser(User user) 
        {
            // When the user already exists.
            if (_usersDictionary.ContainsKey(user.GetUsername()))
            {
                return false;
            }

            _usersDictionary[user.GetUsername()] = user;
            
            // Save the current state to file.
            this._save();

            return true;
        }

        /// <summary>
        /// Authenticates an user with a given username and  password.
        /// </summary>
        /// <param name="username">username of the user to authenticate</param>
        /// <param name="password">password of the user</param>
        /// <returns>true if the authentication was successful. Otherwise
        /// returns false.</returns>
        public bool Authenticate(string username, string password)
        {
            if (_usersDictionary.TryGetValue(username, out User? user))
            {
                return user.GetPassword() == password;
            }
            return false;
        }

        /// <summary>
        /// Saves the state of the users dictionary to output stream.
        /// </summary>
        /// <param name="outputStream">Reference to the output stream.</param>
        /// <returns>true if the save was successful. Otherwise, returns false.</returns>
        private bool _save(TextWriter outputStream)
        {
            // Write the count of users in dictionary.
            // Required while reading them back from file.
            try
            {
                outputStream.WriteLine(_usersDictionary.Count);
            } catch
            {
                return false;
            }

            // Write each user in dictionary.
            foreach (User user in _usersDictionary.Values)
            {
                if (! user.Save(outputStream))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the file path for usersDictiony.
        /// </summary>
        /// <returns>A string containnig the file path.</returns>
        private string _getUsersDictionaryFilePath()
        {
            string dataDirectory = Utils.GetApplicationDirectory();
            string usersDictionaryFilename = Constants.USERS_DICTIONARY_FILENAME;
            return Path.Combine(dataDirectory, usersDictionaryFilename);
        }

        /// <summary>
        /// Saves the current state of the users dictionary to appropriate location
        /// in hard-drive.
        /// </summary>
        /// <returns>true if the save was successful. Otherwise, returns false.</returns>
        private bool _save()
        {
            // Ensure application directory already exists.
            Utils.SetupApplicationDirectory();
            
            string filename = _getUsersDictionaryFilePath();
            TextWriter? outputStream = null;
            try
            {
                outputStream = new StreamWriter(filename);
                if (this._save(outputStream))
                {
                    return true;
                }
                return false;
            } catch
            {
                return false;
            }
            finally
            {
                outputStream?.Close();
            }
        }
    }
}
