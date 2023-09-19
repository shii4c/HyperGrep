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

namespace HyperGrep
{
    public class TextLineViewControl : Canvas
    {
        private Rectangle _keywordRect = new Rectangle();
        private TextBox _textBox = new TextBox();
        public TextLineViewControl()
        {
            ClipToBounds = true;
            SizeChanged += TextLineControl_SizeChanged;

            _keywordRect.Fill = Brushes.Yellow;
            _keywordRect.Visibility = Visibility.Hidden;
            _textBox.IsReadOnly = true;
            _textBox.Background = null;
            _textBox.BorderBrush = null;
            _textBox.BorderThickness = new Thickness(0);
            Children.Add(_keywordRect);
            Children.Add(_textBox);
        }
        public TextLineInfo TextLineInfo
        {
            get { return (TextLineInfo)GetValue(TextLineInfoProperty); }
            set { SetValue(TextLineInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextLineInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextLineInfoProperty =
            DependencyProperty.Register("TextLineInfo", typeof(TextLineInfo), typeof(TextLineViewControl), new PropertyMetadata(null, (d, e) => ((TextLineViewControl)d).TextLineInfoPropertyChanged(e)));

        private void TextLineInfoPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            var text = "";
            var textLineInfo = e.NewValue as TextLineInfo;
            if (textLineInfo != null) { text = textLineInfo.Text; }
            _textBox.Text = text;

            UpdatePosition();
        }

        private void TextLineControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var textLineInfo = TextLineInfo;
            if (textLineInfo == null)
            {
                _keywordRect.Visibility = Visibility.Hidden;
                return;
            }
            _keywordRect.Visibility = Visibility.Visible;

            double leftPos = _textBox.GetRectFromCharacterIndex(textLineInfo.KeywordPosition).Left;
            double rightPos = _textBox.GetRectFromCharacterIndex(textLineInfo.KeywordPosition + textLineInfo.KeywordLength).Right;
            if (double.IsInfinity(rightPos) || double.IsInfinity(leftPos)) { return; }
            double width = ActualWidth;
            double keywordWidth = rightPos - leftPos;
            Canvas.SetLeft(_textBox, width / 2 - leftPos - keywordWidth / 2);
            _keywordRect.Width = keywordWidth;
            _keywordRect.Height = _textBox.ActualHeight;
            Canvas.SetLeft(_keywordRect, (width - keywordWidth) / 2);
            Height = _textBox.ActualHeight;
        }
    }
    public class TextLineInfo
    {
        public TextLineInfo(string text, int keywordPosition, int keywordLength)
        {
            Text = text;
            KeywordPosition = keywordPosition;
            KeywordLength = keywordLength;
        }

        public string Text { get; }

        public int KeywordPosition { get; }

        public int KeywordLength { get; }

        public string Center { get => Text.Substring(KeywordPosition, KeywordLength); }

        public string Left { get => Text.Substring(0, KeywordLength); }

        public string Rigth { get => Text.Substring(0, KeywordPosition + KeywordLength); }
    }
}
