using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonarDatabase.Translate;

namespace DonarDatabase
{
    class DonarDbImp : IDonarDb
    {
        public DonarDbImp()
        {
            translateImp = new TranslateImp(this);
        }

        public string DatabaseFolder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsModified
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsOpen
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SourceLanguage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string TargetLanguage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITranslate Translate
        {
            get
            {
                return translateImp;
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void New(string folderName, string dbName, string sourceLanguage, string targetLanguage)
        {
            throw new NotImplementedException();
        }

        public void Open(string folderName)
        {
            throw new NotImplementedException();
        }

        public void SaveAll()
        {
            throw new NotImplementedException();
        }

        private TranslateImp translateImp;
    }
}
