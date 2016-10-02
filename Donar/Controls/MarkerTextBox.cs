using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Donar.Controls
{
    public class MarkerTextBox : TextBox
    {
        public MarkerTextBox()
        {
            base.Background = null;
        }

        #region Text Dependency Properties
        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        //Using a DependencyProperty as the backing store for Text.This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MarkerTextBox),
                new FrameworkPropertyMetadata {
                    DefaultValue = "",
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                    PropertyChangedCallback = TextPropertyChanged,
                    CoerceValueCallback = TextCoerceValue
                } );

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textbox = (TextBox)d;
            textbox.Text = (string)e.NewValue;
        }
        private static object TextCoerceValue(DependencyObject d, object baseValue)
        {
            MarkerTextBox marker = (MarkerTextBox)d;
            TextBox textbox = (TextBox)d;
            bool oldInvalidParagraph = marker.InvalidParagraph;
            if (baseValue == null)
            {
                marker.InvalidParagraph = true;
                textbox.IsReadOnly = true;
                baseValue = "";
            }
            else
            {
                marker.InvalidParagraph = false;
                if (marker.IsReadOnly != textbox.IsReadOnly)
                {
                    textbox.IsReadOnly = marker.IsReadOnly;
                }
            }
            if (oldInvalidParagraph != marker.InvalidParagraph)
            {
                marker.InvalidateVisual();
            }
            return baseValue;
        }
        #endregion
        #region IsReadOnly Dependency Properties
        public new bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MarkerTextBox), new PropertyMetadata(false, IsReadOnlyPropertyChanged));
        private static void IsReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MarkerTextBox marker = (MarkerTextBox)d;
            TextBox textbox = (TextBox)d;
            if (!marker.InvalidParagraph)
            {
                textbox.IsReadOnly = (bool)e.NewValue;
            }
        }
        #endregion
        #region Background Dependency Properties
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextBackground.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(MarkerTextBox), new PropertyMetadata(Brushes.White));
        #endregion
        #region NoTextBackground Dependency Properties
        public Brush NoTextBackground
        {
            get { return (Brush)GetValue(NoTextBackgroundProperty); }
            set { SetValue(NoTextBackgroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoTextBackgroundProperty =
            DependencyProperty.Register("NoTextBackground", typeof(Brush), typeof(MarkerTextBox), new PropertyMetadata(null));
        #endregion
        #region ParagraphIndex Dependency Properties
        public int ParagraphIndex
        {
            get { return (int)GetValue(ParagraphIndexProperty); }
            set { SetValue(ParagraphIndexProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LineIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParagraphIndexProperty =
            DependencyProperty.Register("ParagraphIndex", typeof(int), typeof(MarkerTextBox), new PropertyMetadata(0));
        #endregion

        #region TextBox events handlers
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            GetRectFromCharacterIndex(0);
            if (!InvalidParagraph) Text = base.Text;
            InvalidateVisual();
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Background != null)
            {
                dc.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
            }
            if (NoTextBackground != null)
            {
                if (InvalidParagraph)
                {
                    dc.DrawRectangle(NoTextBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));
                }
                else
                {
                    Rect r = GetRectFromCharacterIndex(base.Text.Length, true);
                    if (!r.IsEmpty)
                    {
                        dc.DrawRectangle(NoTextBackground, null, new Rect(0, r.Bottom, ActualWidth, Math.Max(ActualHeight - r.Bottom, 0)));
                    }
                }
            }
        }
        #endregion

        #region Private variables
        bool InvalidParagraph = false;
        #endregion
    }
}
