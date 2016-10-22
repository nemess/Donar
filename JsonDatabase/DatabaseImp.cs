using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Donar.Interfaces;

namespace JsonDatabase
{
    [Export(typeof(IDatabase))]
    [ExportMetadata("Name", "Donar JSON Database")]
    class DatabaseImp : IDatabase
    {
        public DatabaseImp()
        {
            translateImp = new TranslateImp(this);
        }

        #region Implement IDatabase interface
        bool IDatabase.IsModified { get { return modified || (translateImp as ITranslate).IsModified; } }
        bool IDatabase.IsOpen { get { return opened; } }
        ITranslate IDatabase.Translate { get { return translateImp; } }
        string IDatabase.Name {
            get { return name; }
            set { name = value; }
        }
        string IDatabase.DefaultExport {
            get { return export; }
            set { export = value; modified = true; }
        }
        string IDatabase.DefaultImport {
            get { return import; }
            set { import = value; modified = true; }
        }
        string IDatabase.Folder { get { return directory; } }

        void IDatabase.Close()
        {
            Close();
        }

        void IDatabase.Create(string folderName, string dbName)
        {
            Create(folderName, dbName);
        }

        void IDatabase.Open(string folderName)
        {
            Open(folderName);
        }

        void IDatabase.SaveAll()
        {
            SaveAll();
        }

        void IDisposable.Dispose()
        {
            Close();
        }
        #endregion

        #region Private Functions
        void Close()
        {
            if (filestream != null)
            {
                filestream.Close();
                filestream = null;
            }
            directory = null;
            filefullpath = null;
            name = null;
            export = null;
            import = null;
            modified = false;

            translateImp.Close();
        }

        void Create(string folderName, string dbName)
        {
            try
            {
                // 1. close currently open database
                Close();
                // 2. Check the given folder
                directory = Directory.CreateDirectory(folderName).FullName;
                // 3. Is there another database
                filefullpath = Path.Combine(directory, FILE_NAME);
                if (File.Exists(filefullpath))
                {
                    throw new ArgumentException("Database is already created in this directory!", "folderName");
                }
                // 4. Create the main files
                filestream = File.Open(filefullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                // 5. Set and save the properties
                name = dbName;
                export = "";
                import = "";
                SaveProperties();
                opened = true;
                // 6. Create empty translate file
                translateImp.Create(directory);
            }
            catch (Exception)
            {
                //string fp = filefullpath;
                Close();
                //if (File.Exists(fp))
                //{
                //    File.Delete(fp);
                //}
                throw;
            }
        }

        void Open(string folderName)
        {
            try
            {
                // 1. close currently open database
                Close();
                // 2. Check the given folder
                directory = Directory.CreateDirectory(folderName).FullName;
                // 3. Is there another database
                filefullpath = Path.Combine(directory, FILE_NAME);
                if (!File.Exists(filefullpath))
                {
                    throw new ArgumentException("Donar project does not exist in this directory!", "folderName");
                }
                // 4. Open the main files
                filestream = File.Open(filefullpath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                // 5. Read properties
                filestream.Position = 0;
                MainRecordJson mainobj = MainRecordJson.ReadObject(filestream);
                name = mainobj.ProjectName;
                export = mainobj.DefaultExport;
                import = mainobj.DefaultImport;

                // 6. Open translate db
                translateImp.Open(directory);
                opened = true;
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        void SaveAll()
        {
            if (!opened) return;
            if (modified)
            {
                SaveProperties();
            }
            if (translateImp.IsModified)
            {
                translateImp.Save();
            }
        }

        void SaveProperties()
        {
            filestream.Position = 0;
            var mainobj = new MainRecordJson();
            mainobj.ProjectName = name;
            mainobj.DefaultExport = export;
            mainobj.DefaultImport = import;
            MainRecordJson.WriteObject(filestream, mainobj);
            filestream.Flush();
            filestream.SetLength(filestream.Position);
            modified = false;
        }
        #endregion

        #region Private variables
        TranslateImp translateImp;
        bool modified = false;
        bool opened = false;
        FileStream filestream = null;
        string filefullpath = null;
        string FILE_NAME = "JsonDatabase.donar";
        string directory = null;
        string name = null;
        string export = null;
        string import = null;
        #endregion
    }
}
