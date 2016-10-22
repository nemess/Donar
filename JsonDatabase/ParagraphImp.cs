using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donar.Interfaces;

namespace JsonDatabase
{
    class ParagraphImp : IParagraph
    {
        public ParagraphImp(TextEntryImp entry, int index)
        {
            textentry = entry;
            ParagraphIndex = index;
            Text = "";
            IsModified = false;
        }

        #region IParagraph implementation
        bool IParagraph.IsModified
        {
            get { return IsModified; }
        }

        string IParagraph.Text
        {
            get { return Text; }
            set { Text = value; IsModified = true; textentry.IsModified = true; }
        }

        int IParagraph.ParagraphIndex
        {
            get { return ParagraphIndex; }
        }

        ITextEntry IParagraph.TextEntry
        {
            get { return textentry; }
        }
        #endregion

        public bool IsModified { get; set; }
        public int ParagraphIndex { get; set; }
        public string Text { get; set; }

        TextEntryImp textentry;
    }
}
