using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerLibrary
{
    /// <summary>
    /// Manages records for one user.
    /// </summary>
    public class RecordManager
    {
        // username of the user whose record is being managed by this instance of RecordManager.
        private readonly string _username;

        // Records dictionary of the user.
        private readonly Dictionary<string, IRecord> _recordsDictionary;

        /// <summary>
        /// Creates a new instance of RecordManager class for a given user.
        /// </summary>
        /// <param name="username">username of the user whose record is to be managed.</param>
        public RecordManager(string username)
        {
            _username = username;
            _recordsDictionary = new Dictionary<string, IRecord>();

            _loadRecords();  // If any.
        }

        /// <summary>
        /// Loads all records to the records dictionary.
        /// </summary>
        /// <returns>true if the load was successful. Otherwise, returns false.</returns>
        private bool _loadRecords()
        {
            string userRecordsFilePath = _getUserRecordsFilePath();
            TextReader? inputStream = null;
            try
            {
                inputStream = new StreamReader(userRecordsFilePath);
                
                // Read records count.
                string recordsCountText = inputStream.ReadLine() ?? string.Empty;
                int recordsCount = int.Parse(recordsCountText);
                
                // Read each record.
                for (int i = 0; i < recordsCount; i++)
                {
                    string recordTypeName = inputStream.ReadLine() ?? string.Empty;
                    IRecord? record = RecordFactory.MakeRecord(recordTypeName, inputStream);
                    
                    if (record == null)
                    {
                        return false;
                    }

                    // Save record to the records dictionary.
                    _recordsDictionary[record.GetWebsite()] = record;
                }
                return true;
            } catch
            {
                return false;
            } finally
            {
                inputStream?.Close();
            }
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
                return _recordsDictionary[website];
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
            _recordsDictionary[record.GetWebsite()] = record;

            //save the current state of record manager to file.
            return _save();
        }

        /// <summary>
        /// Saves the current state of the record manager instance to the output stream.
        /// </summary>
        /// <param name="outputStream">Reference to an output stream to write to.</param>
        /// <returns>true if all records are saved successfully. Otherwise returns false.</returns>
        private bool _save(TextWriter outputStream)
        {
            // Save the total number of records.
            // Necessary for loading the records back in memory.
            outputStream.WriteLine(_recordsDictionary.Count);

            foreach (IRecord record in _recordsDictionary.Values)
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

        /// <summary>
        /// Gets the file path to the records of the user this instance of Record manager
        /// is managing.
        /// </summary>
        /// <returns>A string containing the file path</returns>
        private string _getUserRecordsFilePath()
        {
            string dataDirectory = Utils.GetDataDirectory();
            string userRecordsFilename = _username;
            return Path.Combine(dataDirectory, userRecordsFilename);
        }

        /// <summary>
        /// Saves the current state of record manager to appropriate location
        /// in hard-drive.
        /// </summary>
        /// <returns>true if the save was successful. Otherwise, returns false.</returns>
        private bool _save()
        {
            // Ensure data directory already exists.
            Utils.SetupDataDirectory();

            string filename = _getUserRecordsFilePath();
            TextWriter? outputStream = null;
            try
            {
                outputStream = new StreamWriter(filename);
                if (this._save(outputStream))
                {
                    return true;
                }
                return false;
            } catch
            {
                return false;
            } finally
            {
                outputStream?.Close();
            }
        }
    }
}
