using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PasswordManagerLibrary
{
    public class Authenticator
    {
        // Implement Authenticator as a Singleton.
        private static readonly Authenticator _instance = new();

        /// <summary>
        /// Gets the singleton instance of Authenticator class.
        /// </summary>
        /// <returns>The singleton instance.</returns>
        public static Authenticator GetInstance() { return _instance; }

        private readonly Dictionary<string, User> _usersDictionary;
        private Authenticator()
        {
            _usersDictionary = new Dictionary<string, User>();
            _loadUsers();   // if any.
        }

        // Size of Key and IV for encryption/decryption.
        private const int KEY_BYTES = 32;
        private const int IV_BYTES = 16;
        
        /// <summary>
        /// Loads all users to the users dictionary.
        /// </summary>
        /// <returns>true if the load was successful. Otherwise, returns false.</returns>
        private bool _loadUsers()
        {
            string userDictionaryFilepPath = _getUsersDictionaryFilePath();

            // If users dictionary file doesn't exist, there's nothing to load from.
            if (! File.Exists(userDictionaryFilepPath)) { return false; }

            // Buffers for Key and IV.
            byte[] key = new byte[KEY_BYTES];
            byte[] iv = new byte[IV_BYTES];

            try
            {
                using FileStream fsIn = new(userDictionaryFilepPath, FileMode.Open);
                
                // Read Key and IV.
                if ((fsIn.Read(key, 0, key.Length) != key.Length) ||
                    (fsIn.Read(iv, 0, iv.Length) != iv.Length))
                {
                    return false;   // Couldn't read key and iv successfully.
                }

                // Read rest of the data. I.e. the users dictionary.

                // Add decryption layer.
                using Aes aes = Aes.Create();
                using CryptoStream csIn =
                    new(fsIn, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);

                // Read users dictionary as text.
                using TextReader textReader = new StreamReader(csIn);
                string usersCountText = textReader.ReadLine() ?? string.Empty;
                int usersCount = int.Parse(usersCountText);
                for (int i = 0; i < usersCount; i++)
                {
                    User user = new(textReader);
                    _usersDictionary[user.GetUsername()] = user;
                }
                return true;
            } catch
            {
                return false;
            }
        }


        /// <summary>
        /// Adds an user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if addinig user was successful. Otherwise,
        /// returns false.</returns>
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
                return user.GetPasswordHash()
                    .SequenceEqual(Utils.Hash(password));
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

            try
            {
                // Create a cryptographic object.
                using Aes aes = Aes.Create();

                using FileStream fsOut = new(filename, FileMode.Create);
                
                // Write Key and IV being used to encrypt this file.
                fsOut.Write(aes.Key, 0, aes.Key.Length);
                fsOut.Write(aes.IV, 0, aes.IV.Length);

                // Add encryption layer.
                using CryptoStream csOut = new(fsOut, aes.CreateEncryptor(), CryptoStreamMode.Write);
                
                // Write users dictionary as text.
                using TextWriter textWriter = new StreamWriter(csOut);
                return this._save(textWriter);
            } catch
            {
                return false;
            }
        }
    }
}
