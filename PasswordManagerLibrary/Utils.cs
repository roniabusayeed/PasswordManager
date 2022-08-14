using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    internal static class Utils
    {
        /// <summary>
        /// Gets the root data directory for this application.
        /// </summary>
        /// <returns>A string containing the directory path.</returns>
        internal static string GetApplicationDirectory()
        {
            string directoryLocation = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
            string directoryName = Constants.APPLICATION_DIRECTORY_NAME;
            return Path.Combine(directoryLocation, directoryName);
        }

        /// <summary>
        /// Gets the directory that holds records of all user.
        /// </summary>
        /// <returns>A string containing the directory path.</returns>
        internal static string GetDataDirectory()
        {
            string applicationDirectory = GetApplicationDirectory();
            string dataDirectoryName = Constants.DATA_DIRECTORY_NAME;
            return Path.Combine(applicationDirectory, dataDirectoryName);
        }

        /// <summary>
        /// Creates application directory if it is not found.
        /// </summary>
        internal static void SetupApplicationDirectory()
        {
            if (! Directory.Exists(GetApplicationDirectory()))
            {
                Directory.CreateDirectory(GetApplicationDirectory());
            }
        }

        /// <summary>
        /// Creates data directory inside the application directory, if it
        /// is not found.
        /// </summary>
        internal static void SetupDataDirectory()
        {
            if (! Directory.Exists(GetDataDirectory()))
            {
                Directory.CreateDirectory(GetDataDirectory());
            }
        }
    }
}
