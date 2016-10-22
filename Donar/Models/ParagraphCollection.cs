using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donar.Models
{
    public class ParagraphCollection : ObservableCollection<Paragraph>
    {
        public ParagraphCollection(Interfaces.IUnit u)
        {
            unit = u;
            CreateAll();
        }

        public void InsertNewLine(int paragraphIndex, int charaterIndex)
        {
            Paragraph split = this[paragraphIndex];
            string next = split.Target.Substring(charaterIndex);
            split.Target = split.Target.Substring(0, charaterIndex);

            Interfaces.IParagraph dbprg = unit[Interfaces.TextType.Target].Insert(paragraphIndex+1);
            dbprg.Text = next;

            CreateAll();
        }

        public void RemoveNewLine(int paragraphIndex)
        {
            int removeIndex = paragraphIndex + 1;

            Interfaces.ITextEntry target = unit[Interfaces.TextType.Target];
            Interfaces.IParagraph current = target[paragraphIndex];
            Interfaces.IParagraph remove = target[removeIndex];
            current.Text += remove.Text;

            target.RemoveAt(removeIndex);

            CreateAll();
        }

        void CreateAll()
        {
            Clear();
            int maxIndex = Math.Max(unit[Interfaces.TextType.Source].Count, unit[Interfaces.TextType.Target].Count);
            for (int i = 0; i < maxIndex; ++i)
            {
                Add(new Paragraph(unit, i));
            }
        }

        Interfaces.IUnit unit;
    }
}
