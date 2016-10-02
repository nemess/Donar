using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslateDb = DonarDatabase.Translate;

namespace Donar.Models
{
    public class ParagraphCollection : ObservableCollection<Paragraph>
    {
        public ParagraphCollection(TranslateDb.IUnit u)
        {
            unit = u;
            CreateAll();
        }

        public void InsertNewLine(int paragraphIndex, int charaterIndex)
        {
            Paragraph split = this[paragraphIndex];
            string next = split.Target.Substring(charaterIndex);
            split.Target = split.Target.Substring(0, charaterIndex);

            TranslateDb.IParagraph dbprg = unit[TranslateDb.TextType.Target].Insert(paragraphIndex+1);
            dbprg.Paragraph = next;

            CreateAll();
        }

        public void RemoveNewLine(int paragraphIndex)
        {
            int removeIndex = paragraphIndex + 1;

            TranslateDb.IText target = unit[TranslateDb.TextType.Target];
            TranslateDb.IParagraph current = target[paragraphIndex];
            TranslateDb.IParagraph remove = target[removeIndex];
            current.Paragraph += remove.Paragraph;

            target.RemoveAt(removeIndex);

            CreateAll();
        }

        void CreateAll()
        {
            Clear();
            int maxIndex = Math.Max(unit[TranslateDb.TextType.Source].Count, unit[TranslateDb.TextType.Target].Count);
            for (int i = 0; i < maxIndex; ++i)
            {
                Add(new Paragraph(unit, i));
            }
        }

        TranslateDb.IUnit unit;
    }
}
