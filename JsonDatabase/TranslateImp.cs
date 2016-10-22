using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donar.Interfaces;

namespace JsonDatabase
{
    class TranslateImp : ITranslate
    {
        public TranslateImp(IDatabase db)
        {
            database = db;
            units = new SortedDictionary<string, IUnit>();
        }

        #region ITranslate implementation
        IUnit IDictionary<string, IUnit>.this[string key]
        {
            get { return units[key]; }
            set { throw new NotSupportedException("IUnit cannot be set directly in TranslateImp"); }
        }

        int ICollection<KeyValuePair<string, IUnit>>.Count
        {
            get { return units.Count; }
        }

        IDatabase ITranslate.Database
        {
            get { return database; }
        }

        bool ITranslate.IsModified { get { return modified; } }

        bool ICollection<KeyValuePair<string, IUnit>>.IsReadOnly
        {
            get { return true; }
        }

        ICollection<string> IDictionary<string, IUnit>.Keys
        {
            get { return units.Keys; }
        }

        ICollection<IUnit> IDictionary<string, IUnit>.Values
        {
            get { return units.Values; }
        }

        void ICollection<KeyValuePair<string, IUnit>>.Add(KeyValuePair<string, IUnit> item)
        {
            throw new NotSupportedException("IUnit cannot be added directly in TranslateImp");
        }

        void ITranslate.Add(string unitID)
        {
            Add(unitID);
        }

        void IDictionary<string, IUnit>.Add(string key, IUnit value)
        {
            throw new NotSupportedException("IUnit cannot be added directly in TranslateImp");
        }

        void ICollection<KeyValuePair<string, IUnit>>.Clear()
        {
            throw new NotSupportedException("IUnit cannot be cleared directly in TranslateImp");
        }

        bool ICollection<KeyValuePair<string, IUnit>>.Contains(KeyValuePair<string, IUnit> item)
        {
            return units.Contains(item);
        }

        bool IDictionary<string, IUnit>.ContainsKey(string key)
        {
            return units.ContainsKey(key);
        }

        void ICollection<KeyValuePair<string, IUnit>>.CopyTo(KeyValuePair<string, IUnit>[] array, int arrayIndex)
        {
            throw new NotSupportedException("IUnit cannot be copied directly from TranslateImp");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return units.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, IUnit>> IEnumerable<KeyValuePair<string, IUnit>>.GetEnumerator()
        {
            return units.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, IUnit>>.Remove(KeyValuePair<string, IUnit> item)
        {
            throw new NotSupportedException("IUnit cannot be removed from TranslateImp");
        }

        bool IDictionary<string, IUnit>.Remove(string key)
        {
            throw new NotSupportedException("IUnit cannot be removed from TranslateImp");
        }

        void ITranslate.Save()
        {
            Save();
        }

        bool IDictionary<string, IUnit>.TryGetValue(string key, out IUnit value)
        {
            return units.TryGetValue(key, out value);
        }
        #endregion

        #region Public properties
        public bool IsModified
        {
            get { return modified; }
            set
            {
                modified = value;
                if (!value)
                {
                    foreach (KeyValuePair<string, IUnit> kvp in units)
                    {
                        if (kvp.Value.IsModified)
                        {
                            modified = true;
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Public functions
        public void Create(string folderName)
        {
            try
            {
                Close();
                filefullpath = Path.Combine(folderName, FILE_NAME);
                if (File.Exists(filefullpath))
                {
                    throw new ArgumentException("Database is already created in this directory!", "folderName");
                }
                // create empty file
                FileStream sw = File.OpenWrite(filefullpath);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
                if (filefullpath != null && File.Exists(filefullpath))
                {
                    File.Delete(filefullpath);
                }
                filefullpath = null;
                throw;
            }
        }

        public void Open(string folderName)
        {
            try
            {
                Close();
                filefullpath = Path.Combine(folderName, FILE_NAME);
                if (!File.Exists(filefullpath))
                {
                    throw new ArgumentException("Donar project does not exist in this directory!", "folderName");
                }
                using (FileStream sr = File.OpenRead(filefullpath))
                {
                    memstream.Position = 0;
                    while (true)
                    {
                        int by = sr.ReadByte();
                        if (by < 0) break;
                        else if (by == '\n')
                        {
                            memstream.SetLength(memstream.Position);
                            memstream.Position = 0;
                            TextEntryJson record = TextEntryJson.ReadObject(memstream);
                            memstream.Position = 0;

                            IUnit unit;
                            UnitImp unitimp;
                            if (units.TryGetValue(record.ID, out unit))
                            {
                                unitimp = unit as UnitImp;
                            }
                            else
                            {
                                unitimp = Add(record.ID);
                            }
                            unitimp.SetJsonRecord(record);
                        }
                        else
                        {
                            memstream.WriteByte((byte)by);
                        }
                    }
                    while (sr.Position < sr.Length)
                    {
                        TextEntryJson record = TextEntryJson.ReadObject(sr);
                        IUnit unit;
                        UnitImp unitimp;
                        if (units.TryGetValue(record.ID, out unit))
                        {
                            unitimp = unit as UnitImp;
                        }
                        else
                        {
                            unitimp = Add(record.ID);
                        }
                        unitimp.SetJsonRecord(record);
                    }
                }
            }
            catch(Exception)
            {
                Close();
                throw;
            }
        }

        public void Save()
        {
            using (FileStream sw = File.OpenWrite(filefullpath))
            {
                sw.Position = sw.Length;
                foreach(KeyValuePair<string, IUnit> kv in units)
                {
                    if (kv.Value.IsModified)
                    {
                        (kv.Value as UnitImp).Save(sw);
                    }
                }
            }
        }

        public void Close()
        {
            filefullpath = null;
            units.Clear();
            IsModified = false;
        }

        #endregion

        #region Private functions
        UnitImp Add(string unitID)
        {
            UnitImp unit = new UnitImp(this, unitID);
            units.Add(unitID, unit);
            IsModified = true;
            return unit;
        }
        #endregion

        #region Private variables
        bool modified = false;
        IDatabase database;
        SortedDictionary<string, IUnit> units;
        string filefullpath = null;
        string FILE_NAME = "Translatedb.donar";
        MemoryStream memstream = new MemoryStream();
        #endregion
    }
}
