using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    class TextImp : IText
    {
        public TextImp(UnitImp u, TextType type)
        {
            unit = u;
            Type = type;
            paragraphs = new List<IParagraph>();
        }

        public IParagraph this[int index]
        {
            get
            {
                return paragraphs[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                return paragraphs.Count;
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

        public TextType Type
        {
            get; set;
        }

        public IUnit Unit
        {
            get
            {
                return unit;
            }
        }

        public void Add(IParagraph item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IParagraph item)
        {
            return paragraphs.Contains(item);
        }

        public void CopyTo(IParagraph[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        static private string[] endlines = { "\n", "\r\n", "\r" };
        public void FromString(string str)
        {
            paragraphs.Clear();

            string [] lines = str.Split(endlines, StringSplitOptions.None);
            int lineCount = lines.Count();
            for (int i = 0; i < lineCount; ++i)
            {
                Insert(i).Paragraph = lines[i];
            }
        }

        public IEnumerator<IParagraph> GetEnumerator()
        {
            return paragraphs.GetEnumerator();
        }

        public int IndexOf(IParagraph item)
        {
            return paragraphs.IndexOf(item);
        }

        public IParagraph Insert(int index)
        {
            ParagraphImp pr = new ParagraphImp(this, index);
            paragraphs.Insert(index, pr);
            for (int i = index+1; i < paragraphs.Count; ++i)
            {
                (paragraphs[i] as ParagraphImp).ParagraphIndex = i;
            }
            return pr;
        }

        public void Insert(int index, IParagraph item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IParagraph item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public string ToString(string newLine)
        {
            string ret = "";
            bool first = false;
            foreach (IParagraph prg in paragraphs)
            {
                if (!first) ret += newLine;
                first = false;
                ret += prg.Paragraph;
            }
            return ret;
        }
        public override string ToString()
        {
            return ToString("\r\n");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return paragraphs.GetEnumerator();
        }

        UnitImp unit;
        List<IParagraph> paragraphs;
    }
}
