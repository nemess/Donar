using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    struct debugTestData
    {
        public string id;
        public string source;
        public string target;
    }
    class TranslateImp : ITranslate
    {
        static debugTestData[] testData = new debugTestData[]  
        {
            new debugTestData() {id="Text 01", source="One Line", target="Egy sor"},
            new debugTestData() {id="Text 02", source="One Line", target="Ket\nsor"},
            new debugTestData() {id="Text 03", source="Two\nLine", target="Egy sor"},
            new debugTestData() {id="Text 04", source="Two\nLine", target="Ket\nsor"},
        };
        public TranslateImp(DonarDbImp db)
        {
            donarDb = db;
            units = new SortedDictionary<string, IUnit>();

            // Fill with test data
            foreach (debugTestData dtd in testData)
            {
                UnitImp u = new UnitImp(this, dtd.id);
                u[TextType.Source].FromString(dtd.source);
                u[TextType.Target].FromString(dtd.target);
                units.Add(dtd.id, u);
            }
        }

        public IUnit this[string key]
        {
            get
            {
                return units[key];
            }
            set
            {
                throw new NotSupportedException("IUnit cannot be set directly in TranslateImp");
            }
        }

        public int Count
        {
            get
            {
                return units.Count;
            }
        }

        public IDonarDb Donar
        {
            get
            {
                return donarDb;
            }
        }

        public bool IsModified
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<IUnit> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, IUnit> item)
        {
            throw new NotImplementedException();
        }

        public void Add(string unitID)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, IUnit value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, IUnit> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, IUnit>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, IUnit>> GetEnumerator()
        {
            return units.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, IUnit> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out IUnit value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        DonarDbImp donarDb;
        SortedDictionary<string, IUnit> units;
    }
}
