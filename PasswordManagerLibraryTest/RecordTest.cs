using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PasswordManagerLibrary;

namespace PasswordManagerLibraryTest
{
    [TestClass]
    public class RecordTest     // Tests Record class.
    {
        [TestMethod]
        public void Constructor_InvalidWebsite_Test()
        {
            // Arrange.
            // Invalid website values.
            string websiteEmtpy = string.Empty;
            string websiteOnlyWhitespaces = " \t\n \t";

            // Act and assert.
            Assert.ThrowsException<InvalidWebsiteException>(() =>
            {
                IRecord record = new Record(websiteEmtpy, "username", "password");
            });
            Assert.ThrowsException<InvalidWebsiteException>(() =>
            {
                IRecord record = new Record(websiteOnlyWhitespaces, "username", "password");
            });
        }
        [TestMethod]
        public void Constructor_InvalidUsername_Test()
        {
            // Arrange.
            // Invalid values.
            string usernameEmpty = string.Empty;
            string usernameOnlyWhitespaces = " \n\t   ";
            string usernameWhitespacesInBetween = "user name";

            // Act and assert.
            Assert.ThrowsException<InvalidUsernameException>(() =>
            {
                IRecord record = new Record("website", usernameEmpty, "password");
            });
            Assert.ThrowsException<InvalidUsernameException>(() =>
            {
                IRecord record = new Record("website", usernameOnlyWhitespaces, "password");
            });
            Assert.ThrowsException<InvalidUsernameException>(() =>
            {
                IRecord record = new Record("website", usernameWhitespacesInBetween, "password");
            });
        }
        [TestMethod]
        public void Constructor_InvalidPassword_Test()
        {
            // Arrange.
            string passwordEmpty = string.Empty;

            // Act and assert.
            Assert.ThrowsException<InvalidPasswordException>(() =>
            {
                IRecord record = new Record("website", "username", passwordEmpty);
            });
        }
        [TestMethod]
        public void Constructor_ValidWebsite_Test()
        {
            // Arrange.
            // Valid websites.
            string website = "website";
            string websiteLeadingAndTrailingWhitespaces = "  website\t\t   ";
            string websiteWhitespacesInBetween = "  website name\t\n  ";

            // Act and assert.
            // The following constructor calls shouldn't throw any exceptions.
            IRecord? record = null;
            record = new Record(website, "username", "password");
            record = new Record(
                websiteLeadingAndTrailingWhitespaces,
                "username",
                "password");
            record = new Record(
                websiteWhitespacesInBetween,
                "username",
                "password");
        }
        [TestMethod]
        public void Constructor_ValidUsername_Test()
        {
            // Arrange.
            string username = "username";
            string usernameLeadingAndTrailingWhitespaces = "  \nusername\t  ";

            IRecord? record = null;
            record = new Record("website", username, "password");
            record = new Record(
                "website",
                usernameLeadingAndTrailingWhitespaces,
                "password");
        }
        [TestMethod]
        public void Constructor_ValidPassword_Test()
        {
            // Arrange.
            string password = "password";
            string passwordOnlyWhitespaces = "  \t\n  ";

            // Act and assert.
            IRecord? record = null;
            record = new Record("website", "username", password);
            record = new Record("website", "username", passwordOnlyWhitespaces);
        }
        [TestMethod]
        public void GetWebsite_Test()
        {
            // Arrange.
            // Valid websites inputs and GetWebsite expected return values.
            string website = "website";
            string website_Expected = "website";

            string websiteLeadingAndTrailingWhitespaces = "  website\t\t   ";
            string websiteLeadingAndTrailingWhitespaces_Expected = "website";
            
            string websiteWhitespacesInBetween = "  website name\t\n  ";
            string websiteWhitespacesInBetween_Expected = "website name";

            // Act and assert.
            Assert.AreEqual(
                new Record(website, "username", "password").GetWebsite(),
                website_Expected);
            Assert.AreEqual(
                new Record(websiteLeadingAndTrailingWhitespaces, "username", "password")
                .GetWebsite(),
                websiteLeadingAndTrailingWhitespaces_Expected);
            Assert.AreEqual(
                new Record(websiteWhitespacesInBetween, "username", "password").GetWebsite(),
                websiteWhitespacesInBetween_Expected);
        }
        [TestMethod]
        public void GetUsername_Test()
        {
            // Arrange.
            // Valid username inputs and GetUsername expected return values.
            string username = "username";
            string username_Expected = "username";

            string usernameLeadingAndTrailingWhitespaces = "  \nusername\t  ";
            string usernameLeadingAndTrailingWhitespaces_Expected = "username";

            Assert.AreEqual(
                new Record("website", username, "password").GetUsername(),
                username_Expected);
            Assert.AreEqual(
                new Record("website", usernameLeadingAndTrailingWhitespaces, "password").GetUsername(),
                usernameLeadingAndTrailingWhitespaces_Expected);
        }
        [TestMethod]
        public void GetPassword_Test()
        {
            // Arrange.
            // Valid password inputs and GetPassword expected return values.
            string password = "password";
            string password_Expected = password;

            string passwordOnlyWhitespaces = "  \t\n  ";
            string passwordOnlyWhitespaces_Expected = passwordOnlyWhitespaces;

            // Act and assert.
            Assert.AreEqual(
                new Record("website", "username", password).GetPassword(),
                password_Expected);
            Assert.AreEqual(
                new Record("website", "username", passwordOnlyWhitespaces).GetPassword(),
                passwordOnlyWhitespaces_Expected);
        }
        [TestMethod]
        public void Constructor_InputStream_Test_1()
        {
            const string filename = "TestInputFiles/Record_Constructor_InputStream_Test_1.txt";
            TextReader inputStream = new StreamReader(filename);
            
            // Try to instantiate off of a closed stream should
            // result in an Exception instance being thrown.
            inputStream.Close();
            Assert.ThrowsException<Exception>(() =>
            {
                new Record(inputStream);
            });
        }
        [TestMethod]
        public void Constructor_InputStream_Test_2()
        {
            const string filename = "TestInputFiles/Record_Constructor_InputStream_Test_2.txt";
            TextReader inputStream = new StreamReader(filename);
            
            Assert.ThrowsException<InvalidWebsiteException>(() =>
            {
                new Record(inputStream);
            });
        }
        [TestMethod]
        public void Constructor_InputStream_Test_3()
        {
            const string filename = "TestInputFiles/Record_Constructor_InputStream_Test_3.txt";
            TextReader inputStream = new StreamReader(filename);

            Assert.ThrowsException<InvalidUsernameException>(() =>
            {
                new Record(inputStream);
            });
        }
        [TestMethod]
        public void Constructor_InputStream_Test_4()
        {
            const string filename = "TestInputFiles/Record_Constructor_InputStream_Test_4.txt";
            TextReader inputStream = new StreamReader(filename);

            Assert.ThrowsException<InvalidPasswordException>(() =>
            {
                new Record(inputStream);
            });
        }
        [TestMethod]
        public void Constructor_InputStream_Test_5()
        {
            const string filename = "TestInputFiles/Record_Constructor_InputStream_Test_5.txt";
            TextReader inputStream = new StreamReader(filename);

            new Record(inputStream);
            inputStream.Close();
        }
    }
}
