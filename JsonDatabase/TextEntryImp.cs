using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donar.Interfaces;

namespace JsonDatabase
{
    class TextEntryImp : ITextEntry
    {
        public TextEntryImp(UnitImp u, TextType type)
        {
            unit = u;
            txttype = type;
            paragraphs = new List<IParagraph>();
        }

        #region IText implementation
        IParagraph IList<IParagraph>.this[int index]
        {
            get { return paragraphs[index]; }
            set { throw new NotSupportedException("IParagraph cannot be set directly in TextImp"); }
        }

        int ICollection<IParagraph>.Count
        {
            get { return paragraphs.Count; }
        }

        bool ITextEntry.IsModified
        {
            get { return modified; }
        }

        bool ICollection<IParagraph>.IsReadOnly
        {
            get { return true; }
        }

        TextType ITextEntry.Type
        {
            get { return txttype;  }
        }

        IUnit ITextEntry.Unit
        {
            get { return unit; }
        }

        void ICollection<IParagraph>.Add(IParagraph item)
        {
            throw new NotSupportedException("IParagraph cannot be added directly in TextImp");
        }

        void ICollection<IParagraph>.Clear()
        {
            throw new NotSupportedException("IParagraph cannot be cleared directly in TextImp");
        }

        bool ICollection<IParagraph>.Contains(IParagraph item)
        {
            return paragraphs.Contains(item);
        }

        void ICollection<IParagraph>.CopyTo(IParagraph[] array, int arrayIndex)
        {
            throw new NotSupportedException("IParagraph cannot be coped directly in TextImp");
        }

        static private string[] endlines = { "\n", "\r\n", "\r" };
        void ITextEntry.FromString(string str)
        {
            FromString(str);
        }

        public IEnumerator<IParagraph> GetEnumerator()
        {
            return paragraphs.GetEnumerator();
        }

        int IList<IParagraph>.IndexOf(IParagraph item)
        {
            return paragraphs.IndexOf(item);
        }

        IParagraph ITextEntry.Insert(int index)
        {
            return Insert(index);
        }

        void IList<IParagraph>.Insert(int index, IParagraph item)
        {
            throw new NotSupportedException("IParagraph cannot be inserted directly into TextImp");
        }

        bool ICollection<IParagraph>.Remove(IParagraph item)
        {
            return paragraphs.Remove(item);
        }

        void IList<IParagraph>.RemoveAt(int index)
        {
            paragraphs.RemoveAt(index);
        }

        string ITextEntry.ToString(string newLine)
        {
            return ToString(newLine);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return paragraphs.GetEnumerator();
        }
        #endregion

        public bool IsModified
        {
            get { return modified; }
            set
            {
                modified = value;
                if (value) unit.IsModified = true;
                else
                {
                    foreach (IParagraph iprg in paragraphs)
                    {
                        (iprg as ParagraphImp).IsModified = false;
                    }
                }
            }
        }

        #region Public functions
        public void FromString(string str)
        {
            paragraphs.Clear();

            string[] lines = str.Split(endlines, StringSplitOptions.None);
            int lineCount = lines.Count();
            for (int i = 0; i < lineCount; ++i)
            {
                ((IParagraph)Insert(i)).Text = lines[i];
            }
            IsModified = true;
        }

        public override string ToString()
        {
            return ToString("\r\n");
        }

        public string ToString(string newLine)
        {
            string ret = "";
            bool first = false;
            foreach (IParagraph prg in paragraphs)
            {
                if (!first) ret += newLine;
                first = false;
                ret += prg.Text;
            }
            return ret;
        }

        public ParagraphImp Insert(int index)
        {
            ParagraphImp pr = new ParagraphImp(this, index);
            paragraphs.Insert(index, pr);
            for (int i = index + 1; i < paragraphs.Count; ++i)
            {
                (paragraphs[i] as ParagraphImp).ParagraphIndex = i;
            }
            return pr;
        }

        public TextEntryJson ToJsonEntry()
        {
            TextEntryJson ret = new TextEntryJson();
            ret.ID = (unit as IUnit).UnitID;
            ret.type = txttype;
            ret.lines = new string[paragraphs.Count];
            for (int i = 0; i < paragraphs.Count; ++i)
            {
                ret.lines[i] = paragraphs[i].Text;
            }
            return ret;
        }

        public void SetAllParagraphes(string[] lines)
        {
            for (int i = 0; i < lines.Count(); ++i)
            {
                ParagraphImp pim;
                if (i < paragraphs.Count)
                {
                    pim = (ParagraphImp)paragraphs[i];
                }
                else
                {
                    pim = Insert(i);
                }
                pim.Text = lines[i];
            }
            while (paragraphs.Count > lines.Count())
            {
                paragraphs.RemoveAt(paragraphs.Count-1);
            }
        }
        #endregion

        #region Private variables
        UnitImp unit;
        TextType txttype;
        List<IParagraph> paragraphs;
        bool modified = false;
        #endregion
    }
}
