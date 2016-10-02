using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase
{
    public interface IDonarDb
    {
        /// <summary>
        /// Gets the open status of database.
        /// </summary>
        bool IsOpen { get; }

        string Name { get; }
        string SourceLanguage { get; }
        string TargetLanguage { get; }
        string DatabaseFolder { get; }

        bool IsModified { get; }

        /// <summary>
        /// Get the translation database.
        /// </summary>
        Translate.ITranslate Translate { get; }

        /// <summary>
        /// It creates a new empty database.
        /// If the database was opened, it will be closed.
        /// </summary>
        /// <param name="folderName">The folder name where the database will created. 
        /// It should be empty. It will be created if it does not exist.</param>
        /// <param name="dbName">Name of the database</param>
        /// <param name="sourceLanguage">Source languge code. Example: en_US</param>
        /// <param name="targetLanguage">Target language code. Example: de_DE</param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        void New(string folderName, string dbName, string sourceLanguage, string targetLanguage);

        /// <summary>
        /// Open the database which assosiated to the given folder.
        /// If the database was opened, it will be closed.
        /// </summary>
        /// <param name="folderName">database folder name</param>
        void Open(string folderName);

        /// <summary>
        /// Save all not saved data
        /// </summary>
        void SaveAll();

        /// <summary>
        /// It closes the database. Not saved data will be lost.
        /// </summary>
        void Close();

    }
}
