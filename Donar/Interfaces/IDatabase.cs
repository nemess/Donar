using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Interfaces
{
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Gets the open status of database.
        /// </summary>
        bool IsOpen { get; }

        string Name { get; set;  }
        string DefaultExport { get; set; }
        string DefaultImport { get; set; }
        string Folder { get; }

        bool IsModified { get; }

        /// <summary>
        /// Get the translation database.
        /// </summary>
        ITranslate Translate { get; }

        /// <summary>
        /// It creates a new empty database.
        /// If the database was opened, it will be closed.
        /// </summary>
        /// <param name="folderName">The folder name where the database will created. 
        /// It should be empty. It will be created if it does not exist.</param>
        /// <param name="dbName">Name of the database</param>
        void Create(string folderName, string dbName);

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
