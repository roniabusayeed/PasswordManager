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
    }
}
