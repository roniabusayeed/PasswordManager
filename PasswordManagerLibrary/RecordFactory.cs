namespace PasswordManagerLibrary
{
    public class RecordFactory
    {
        /// <summary>
        /// Makes a new instance of typeName from inputStream.
        /// </summary>
        /// <param name="typeName">Name of the type of the object to instantiate.</param>
        /// <param name="inputStream">Reference to an open input stream to read from.</param>
        /// <returns>Returns a reference to the instantiated object. Otherwise, returns
        /// null if the instantiation was not successful.</returns>
        public static IRecord? MakeRecord(string typeName, TextReader inputStream)
        {
            IRecord? record = null;
            switch (typeName)
            {
                case "Record":
                    try
                    {
                        record = new Record(inputStream);
                    }
                    catch { record = null; }
                    break;
            }
            return record;
        }
    }
}
