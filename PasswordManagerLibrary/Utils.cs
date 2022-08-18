using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerLibrary
{
    public static class Utils
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
            if (!Directory.Exists(GetApplicationDirectory()))
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
            if (!Directory.Exists(GetDataDirectory()))
            {
                Directory.CreateDirectory(GetDataDirectory());
            }
        }

        /// <summary>
        /// Computes hash of a string.
        /// </summary>
        /// <param name="str">A string whose hash is to be computed</param>
        /// <returns>A byte array containing the hash</returns>
        internal static byte[] Hash(string str)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Generates a password.
        /// </summary>
        /// <returns></returns>
        public static string GeneratePassword(
            int numberOfLettersMin = 8,
            int numberOfLettersMax = 12,
            int numberOfDigitsMin = 2,
            int numberOfDigitsMax = 4,
            int numberOfSymbolsMin = 2,
            int numberOfSymbolsMax = 4)
        {
            const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string DIGITS = "0123456789";
            const string SYMBOLS = "!#$%&()*+";

            StringBuilder passwordBuilder = new();
            Random random = new();

            // Add letters.
            for (int i = 0, n = random.Next(numberOfLettersMin, numberOfLettersMax + 1);
                i < n; i++)
            {
                passwordBuilder.Append(LETTERS[random.Next(LETTERS.Length)]);
            }

            // Add digits.
            for (int i = 0, n = random.Next(numberOfDigitsMin, numberOfDigitsMax + 1);
                i < n; i++)
            {
                passwordBuilder.Append(DIGITS[random.Next(DIGITS.Length)]);
            }

            // Add symbols.
            for (int i = 0, n = random.Next(numberOfSymbolsMin, numberOfSymbolsMax + 1);
                i < n; i++)
            {
                passwordBuilder.Append(SYMBOLS[random.Next(SYMBOLS.Length)]);
            }

            // Shuffle the string.
            var shuffledPassword = passwordBuilder.ToString()
                .OrderBy(ch => random.Next());

            return string.Join(null, shuffledPassword);
        }
    }
}
