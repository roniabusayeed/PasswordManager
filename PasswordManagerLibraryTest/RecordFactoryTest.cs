using PasswordManagerLibrary;

namespace PasswordManagerLibraryTest
{
    [TestClass]
    public class RecordFactoryTest
    {
        [TestMethod]
        public void MakeRecord_ValidInputs_Test()
        {
            // Arrange.
            const string filename = "TestInputFiles/MakeRecord_Test_1.txt";
            TextReader inputStream = new StreamReader(filename);

            // Act.
            IRecord? record = RecordFactory.MakeRecord("Record", inputStream);
            inputStream.Close();

            // Assert.
            Assert.IsNotNull(record);
            Assert.AreEqual(record.GetWebsite(), "web   site");
            Assert.AreEqual(record.GetUsername(), "username");
            Assert.AreEqual(record.GetPassword(), "pass word   ");
        }
        [TestMethod]
        public void MakeRecord_InvalidInputs_Test()
        {
            // Arrange.
            const string filename = "TestInputFiles/MakeRecord_Test_2.txt";
            TextReader inputStream = new StreamReader(filename);

            // Act.
            IRecord? record = RecordFactory.MakeRecord("Record", inputStream);
            inputStream.Close();

            // Assert.
            Assert.IsNull(record);
        }
    }
}
