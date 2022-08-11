using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PasswordManagerLibrary;

namespace PasswordManagerLibraryTest
{
    [TestClass]
    public class RecordManagerTest
    {
        [TestMethod]
        public void FindRecord_Empty_Test()
        {
            // Arrange.
            RecordManager manager = new();

            //Act.
            IRecord? result = manager.FindRecord("website");

            // Assert.
            Assert.IsNull(result);
        }
        [TestMethod]
        public void FindRecord_NonEmpty_Test_1()
        {
            // Arrange.
            RecordManager manager = new();
            IRecord record = new Record("website", "username", "password");
            manager.AddRecord(record);

            // Act.
            IRecord? foundRecord = manager.FindRecord("website");

            // Assert.
            Assert.IsNotNull(foundRecord);
            Assert.AreEqual(foundRecord, record);
        }
        [TestMethod]
        public void FindRecord_NonEmpty_Test2()
        {
            // Arrange.
            RecordManager manager = new();
            IRecord record = new Record("website", "username", "password");
            manager.AddRecord(record);

            // Act.
            IRecord? foundRecord = manager.FindRecord("non-existent website");

            // Assert.
            Assert.IsNull(foundRecord);
        }
        [TestMethod]
        public void AddRecord_Empty_Test()
        {
            // Arrange.
            RecordManager manager = new();
            IRecord record = new Record("website", "username", "password");

            // Act.
            bool result = manager.AddRecord(record);

            // Assert.
            Assert.IsTrue(result);
            IRecord? foundRecord = manager.FindRecord(record.GetWebsite());
            Assert.IsNotNull(foundRecord);
            Assert.AreEqual(foundRecord, record);
        }
        [TestMethod]
        public void AddRecord_NonEmpty_Test_1()
        {
            // Arrange.
            RecordManager manager = new();
            IRecord record = new Record("website", "username", "password");
            manager.AddRecord(record);
            IRecord record2 = new Record("website2", "username2", "password2");

            // Act and assert.
            bool result = manager.AddRecord(record2);
            Assert.IsTrue(result);
            IRecord? foundRecord = manager.FindRecord(record2.GetWebsite());
            Assert.IsNotNull(foundRecord);
            Assert.AreEqual(foundRecord, record2);
        }
        [TestMethod]
        public void AddRecord_NonEmpty_Test_2()
        {
            // Arrange.
            RecordManager manager = new();
            const string website = "website";
            const string modified_username = "modified-username";
            const string modified_password = "modified-password";
            IRecord record = new Record(website, "username", "password");
            manager.AddRecord(record);
            IRecord record2 = new Record(website, modified_username, modified_password);

            // Act.
            manager.AddRecord(record2);

            // Assert.
            IRecord? foundRecord = manager.FindRecord(website);
            Assert.IsNotNull(foundRecord);
            Assert.AreEqual(foundRecord.GetUsername(), modified_username);
            Assert.AreEqual(foundRecord.GetPassword(), modified_password);
        }
        [TestMethod]
        public void Save_Test()
        {
            // Arrange.
            RecordManager manager = new();
            manager.AddRecord(new Record("website", "username", "password"));
            manager.AddRecord(new Record("website", "modified-username", "modified-password"));
            manager.AddRecord(new Record("website2", "username2", "password2"));
            const string saveFileName = "TestOutputFiles/saveFile.txt";
            TextWriter outputStream = new StreamWriter(saveFileName);
            bool result = manager.Save(outputStream);
            outputStream.Close();

            // Assert.
            Assert.IsTrue(result);
        }
    }
}
