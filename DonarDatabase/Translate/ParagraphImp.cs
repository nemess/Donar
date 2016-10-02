using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase.Translate
{
    class ParagraphImp : IParagraph
    {
        public ParagraphImp(TextImp txt, int index)
        {
            text = txt;
            ParagraphIndex = index;
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

        public string Paragraph
        {
            get; set;
        }

        public int ParagraphIndex
        {
            get; set;
        }

        public IText Text
        {
            get
            {
                return text;
            }
        }

        TextImp text;
    }
}
