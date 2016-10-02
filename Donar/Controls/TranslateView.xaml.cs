using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Donar.Controls
{
    /// <summary>
    /// Interaction logic for TranslateView.xaml
    /// </summary>
    public partial class TranslateView : UserControl, INotifyPropertyChanged
    {
        public TranslateView()
        {
            ParagraphCollection = null;
            InitializeComponent();
            ParagraphListBox.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        #region Public Properties
        Models.ParagraphCollection paragraphCollection;
        public Models.ParagraphCollection ParagraphCollection {
            get { return paragraphCollection; }
            set { paragraphCollection = value; NotifyPropertyChanged("ParagraphCollection"); }
        }

        public event PropertyChangedEventHandler PropertyChanged = null;
        #endregion

        public void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Event handlers from ListBox
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParagraphListBox.SelectedIndex < 0) return;
            MarkerTextBox mkbox = (MarkerTextBox)FindListBoxTemplateObject(ParagraphListBox.SelectedIndex, "Target");
            if (IsFocused)
            {
                if (mkbox != null)
                {
                    mkbox.Focus();
                    Keyboard.Focus(mkbox);
                    IncompliteFocus = -1;
                }
                else
                {
                    IncompliteFocus = ParagraphListBox.SelectedIndex;
                }
            }
        }
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (IncompliteFocus >= 0 &&
                ParagraphListBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                MarkerTextBox mkbox = (MarkerTextBox)FindListBoxTemplateObject(IncompliteFocus, "Target");
                if (mkbox != null)
                {
                    mkbox.Focus();
                    Keyboard.Focus(mkbox);
                    IncompliteFocus = -1;
                }
            }
        }
        #endregion

        #region Event handlers from MarkerTextBox
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            int lineid = ((MarkerTextBox)sender).ParagraphIndex;
            ParagraphListBox.SelectedIndex = lineid;
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MarkerTextBox mkrbox = (MarkerTextBox)sender;
            double caretpos = -1;
            bool caretUp = false; // caret is moving up
            // shift, alt ot ctrl is pressed
            if (Keyboard.IsKeyUp(Key.LeftShift) && Keyboard.IsKeyUp(Key.RightShift) &&
                Keyboard.IsKeyUp(Key.LeftAlt) && Keyboard.IsKeyUp(Key.RightAlt) &&
                Keyboard.IsKeyUp(Key.LeftCtrl) && Keyboard.IsKeyUp(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (mkrbox.GetLineIndexFromCharacterIndex(mkrbox.CaretIndex) == 0)
                        {
                            caretpos = GetCaretXPosition(mkrbox);
                            caretUp = true;
                            e.Handled = true;
                        }
                        break;

                    case Key.Down:
                        if (mkrbox.Text == null || 
                            mkrbox.GetLineIndexFromCharacterIndex(mkrbox.CaretIndex) == mkrbox.GetLineIndexFromCharacterIndex(mkrbox.Text.Length))
                        {
                            caretpos = GetCaretXPosition(mkrbox);
                            caretUp = false;
                            e.Handled = true;
                        }
                        break;

                    case Key.Left:
                        if (mkrbox.CaretIndex == 0)
                        {
                            caretpos = ActualWidth;
                            caretUp = true;
                            e.Handled = true;
                        }
                        break;

                    case Key.Right:
                        if (mkrbox.Text == null ||
                            mkrbox.CaretIndex == mkrbox.Text.Length)
                        {
                            caretpos = 0;
                            caretUp = false;
                            e.Handled = true;
                        }
                        break;

                    case Key.Enter:
                        if (!mkrbox.IsReadOnly && mkrbox.Text != null)
                        {
                            mkrbox.GetBindingExpression(MarkerTextBox.TextProperty).UpdateSource();
                            InsertNewLine(mkrbox.ParagraphIndex, mkrbox.CaretIndex);
                        }
                        e.Handled = true;
                        break;

                    case Key.Back:
                        if (!mkrbox.IsReadOnly && mkrbox.Text != null && mkrbox.CaretIndex == 0)
                        {
                            mkrbox.GetBindingExpression(MarkerTextBox.TextProperty).UpdateSource();
                            RemoveNewLine(mkrbox.ParagraphIndex - 1);
                            e.Handled = true;
                        }
                        break;

                    case Key.Delete:
                        if (!mkrbox.IsReadOnly && mkrbox.Text != null && mkrbox.CaretIndex == mkrbox.Text.Length)
                        {
                            mkrbox.GetBindingExpression(MarkerTextBox.TextProperty).UpdateSource();
                            RemoveNewLine(mkrbox.ParagraphIndex);
                            e.Handled = true;
                        }
                        break;
                }
            }
            if (caretpos >= 0)
            {
                int newLineIndex = mkrbox.ParagraphIndex + (caretUp ? -1 : 1);
                MoveCareToOtherBox(newLineIndex, caretpos, caretUp, mkrbox.Name);
            }
        }
        private void MoveCareToOtherBox(int lineIndex, double caretpos, bool caretUp, string boxName)
        {
            if (lineIndex < 0 || lineIndex >= ParagraphListBox.Items.Count)
            {
                return;
            }
            MarkerTextBox newmkrbox = (MarkerTextBox)FindListBoxTemplateObject(lineIndex, boxName);
            newmkrbox.Focus();
            Keyboard.Focus(newmkrbox);

            int newTextLen = newmkrbox.Text != null ? newmkrbox.Text.Length : 0;
            Rect r = newmkrbox.GetRectFromCharacterIndex(caretUp ? newTextLen : 0);
            int ind = newmkrbox.GetCharacterIndexFromPoint(new Point(caretpos, (r.Bottom + r.Top) / 2), true);
            r = newmkrbox.GetRectFromCharacterIndex(ind, false);
            Rect rTrail = newmkrbox.GetRectFromCharacterIndex(ind, true);
            if (Math.Abs(caretpos - r.Left) > Math.Abs(caretpos - rTrail.Left))
            {
                ++ind;
            }
            newmkrbox.CaretIndex = ind;
        }
        private void InsertNewLine(int lineindex, int charaterIndex)
        {
            ParagraphCollection.InsertNewLine(lineindex, charaterIndex);

            NotifyPropertyChanged("ParagraphCollection");
            ParagraphListBox.SelectedIndex = lineindex + 1;
            ParagraphListBox.ScrollIntoView(ParagraphListBox.SelectedItem);
        }
        private void RemoveNewLine(int lineindex)
        {
            int nextline = lineindex + 1;
            if (lineindex < 0 || nextline >= ParagraphCollection.Count ||
                ParagraphCollection[lineindex].Target == null ||
                ParagraphCollection[nextline].Target == null) return;

            int newcaretindex = ParagraphCollection[lineindex].Target.Length;

            ParagraphCollection.RemoveNewLine(lineindex);

            NotifyPropertyChanged("ParagraphCollection");
            ParagraphListBox.SelectedIndex = lineindex;
            ParagraphListBox.ScrollIntoView(ParagraphListBox.SelectedItem);
            MarkerTextBox mkbox = (FindListBoxTemplateObject(lineindex, "Target") as MarkerTextBox);
            mkbox.Focus();
            Keyboard.Focus(mkbox);
            mkbox.CaretIndex = newcaretindex;
        }
        #endregion

        #region General Private Functions
        private double GetCaretXPosition(MarkerTextBox mkrbox)
        {
            Rect r = mkrbox.GetRectFromCharacterIndex(mkrbox.CaretIndex);
            if (r.IsEmpty) return 0;
            return r.Left;
        }
        private object FindListBoxTemplateObject(int index, string name)
        {
            ListBoxItem lbitem = ParagraphListBox.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
            if (lbitem == null) return null;
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(lbitem);
            if (myContentPresenter == null) return null;
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            return myDataTemplate.FindName(name, myContentPresenter);
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion

        int IncompliteFocus = -1;
    }
}
