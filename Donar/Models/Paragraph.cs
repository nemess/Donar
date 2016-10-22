using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Donar.Models
{
    public class Paragraph : INotifyPropertyChanged
    {

        public Paragraph(Interfaces.IUnit u, int index)
        {
            unit = u;
            ParagraphIndex = index < 0 ? 0 : index;
        }
        public int ParagraphIndex { get; private set; }
        public string Source {
            get { return getString(Interfaces.TextType.Source); } }
        public string Target {
            get {
                return getString(Interfaces.TextType.Target);
            }
            set {
                if (ParagraphIndex < unit[Interfaces.TextType.Target].Count)
                {
                    unit[Interfaces.TextType.Target][ParagraphIndex].Text = value;
                    NotifyPropertyChanged("Target");
                }
            }
        }

        public void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = null;


        string getString(Interfaces.TextType type)
        {
            if (ParagraphIndex >= unit[type].Count)
            {
                return null;
            }
            return unit[type][ParagraphIndex].Text;
        }

        Interfaces.IUnit unit;
    }
}
