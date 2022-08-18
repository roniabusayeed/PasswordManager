namespace PasswordManagerLibrary
{
    /// <summary>
    /// Base exception class for everything password manager library.
    /// </summary>
    public class PasswordManagerException : Exception
    {
        public PasswordManagerException() { }
        public PasswordManagerException(string message) : base(message) { }
    }

    public class InvalidWebsiteException : PasswordManagerException
    {
        public InvalidWebsiteException() { }
        public InvalidWebsiteException(string message) : base(message) { }
    }

    public class InvalidUsernameException : PasswordManagerException
    {
        public InvalidUsernameException() { }
        public InvalidUsernameException(string message) : base(message) { }
    }

    public class InvalidPasswordException : PasswordManagerException
    {
        public InvalidPasswordException() { }
        public InvalidPasswordException(string message) : base(message) { }
    }
}
