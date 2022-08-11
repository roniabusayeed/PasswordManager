using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    public class RecordManager
    {
        private readonly Dictionary<string, IRecord> _records;

        /// <summary>
        /// Creates a new instance of the RecordManager class.
        /// </summary>
        public RecordManager()
        {
            _records = new Dictionary<string, IRecord>();
        }

        /// <summary>
        /// Finds the record for a given a website.
        /// </summary>
        /// <param name="website">A string containing the name of the website</param>
        /// <returns>A reference to the Record instance if it is found. Otherwise
        /// returns null.</returns>
        public IRecord? FindRecord(string website)
        {
            try
            {
                return _records[website];
            } catch (KeyNotFoundException)
            {
                return null;
            }          
        }

        /// <summary>
        /// Adds a new record to this RecordManager instance. If there is
        /// already a record with identical Website, it updates the record.
        /// </summary>
        /// <param name="record">Reference to the record instance.</param>
        /// <returns>true if the record is added successfully. Otherwise,
        /// returns false.</returns>
        public bool AddRecord(IRecord record)
        {
            _records[record.GetWebsite()] = record;
            return true;
        }

        /// <summary>
        /// Saves a record manager instance to a output stream.
        /// </summary>
        /// <param name="outputStream">Reference to an output stream to write to.</param>
        /// <returns>true if all records are saved successfully. Otherwise returns false.</returns>
        public bool Save(TextWriter outputStream)
        {
            // Save the total number of records.
            // Necessary for loading the records back in memory.
            outputStream.WriteLine(_records.Count);

            foreach (IRecord record in _records.Values)
            {
                // Save the name of the IRecord type.
                // Necessary for loading them back in memory when
                // more than one type of record implementing IRecord
                // interface.
                outputStream.WriteLine(record.GetType().Name);

                // If any record is not saved successfully,
                // cease save operation and return false.
                if (! record.Save(outputStream))
                {
                    return false;
                }
            }

            // All records are saved successfully.
            return true;
        }
    }
}
