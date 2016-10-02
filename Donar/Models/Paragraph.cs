using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TranslateDb = DonarDatabase.Translate;

namespace Donar.Models
{
    public class Paragraph : INotifyPropertyChanged
    {

        public Paragraph(TranslateDb.IUnit u, int index)
        {
            unit = u;
            ParagraphIndex = index < 0 ? 0 : index;
        }
        public int ParagraphIndex { get; private set; }
        public string Source {
            get { return getString(TranslateDb.TextType.Source); } }
        public string Target {
            get {
                return getString(TranslateDb.TextType.Target);
            }
            set {
                if (ParagraphIndex < unit[TranslateDb.TextType.Target].Count)
                {
                    unit[TranslateDb.TextType.Target][ParagraphIndex].Paragraph = value;
                    NotifyPropertyChanged("Target");
                }
            }
        }

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = null;


        string getString(TranslateDb.TextType type)
        {
            if (ParagraphIndex >= unit[type].Count)
            {
                return null;
            }
            return unit[type][ParagraphIndex].Paragraph;
        }

        TranslateDb.IUnit unit;
    }
}
