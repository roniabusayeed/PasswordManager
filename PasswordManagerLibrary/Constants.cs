using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    public static class Constants
    {
        internal const string APPLICATION_DIRECTORY_NAME = "PasswordManager";
        internal const string USERS_DICTIONARY_FILENAME = "usersDictionary";
        internal const string DATA_DIRECTORY_NAME = "Data";
        public const string INVALID_LOGIN_MESSAGE = "Incorrect username or password";
        public const string APPLICATION_NAME = "Password Manager";
        public const string SIGN_UP_REQUIREMENTS
            = "Username and/or password doesn't meet the requirements.\n\n" +
            "Username requirements:\n" +
            "\tUsername cannot be empty.\n" +
            "\tShould contain only letters, numbers, and underscore.\n" +
            "Password requirements:\n" +
            "\tOne or more lowecase letters.\n" +
            "\tOne or more uppercase letters.\n" +
            "\tOne or more digits.\n" +
            "\tAt least 8 characters.";
    }
}
