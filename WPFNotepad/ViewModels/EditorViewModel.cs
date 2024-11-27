using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WPFNotepad.Models;

namespace WPFNotepad.ViewModels
{
    public class EditorViewModel : ObservableObject
    {
        private readonly RichTextBox _richTextBox;
        private static readonly int DefaultZoomLevel = ConfigurationHelper.DefaultZoomLevel;
        private static readonly int MinZoomLevel = ConfigurationHelper.MinZoomLevel;
        private static readonly int MaxZoomLevel = ConfigurationHelper.MaxZoomLevel;
        private int _zoomLevel;
        public ICommand FormatCommand { get; }
        public ICommand WrapCommand { get; }
        public FormatModel Format { get; set; }
        public DocumentModel Document { get; set; }

        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand ChangeTextColorCommand { get; }
        public ICommand ZoomInCommand { get; set; }
        public ICommand ZoomOutCommand { get; set; }
        public ICommand RestoreZoomCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public double ZoomLevelRatio => ZoomLevel / (double)DefaultZoomLevel;
        private Stack<string> _undoStack;

        public EditorViewModel(DocumentModel document, RichTextBox richTextBox)
        {
            Document = document;
            Format = new FormatModel();
            _richTextBox = richTextBox;
            _undoStack = new Stack<string>();
            ZoomLevel = DefaultZoomLevel;
            FormatCommand = new RelayCommand(OpenStyleDialog);
            WrapCommand = new RelayCommand(ToggleWrap);
            CutCommand = new RelayCommand(CutText);
            CopyCommand = new RelayCommand(CopyText);
            PasteCommand = new RelayCommand(PasteText);
            UndoCommand = new RelayCommand(UndoText);
            ChangeTextColorCommand = new RelayCommand(ChangeTextColor);
            RestoreZoomCommand = new RelayCommand(RestoreZoom);
            ZoomInCommand = new RelayCommand(ZoomIn);
            ZoomOutCommand = new RelayCommand(ZoomOut);
            Document.PropertyChanged += Document_PropertyChanged;
        }

        private void Document_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Document.Text))
            {
                _richTextBox.Document.Blocks.Clear();
                _richTextBox.Document.Blocks.Add(new Paragraph(new Run(Document.Text)));
                _undoStack.Push(Document.Text);
            }
        }

        public int ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                if (_zoomLevel != value)
                {
                    int oldValue = _zoomLevel;
                    _zoomLevel = value;
                    OnPropertyChanged(ref oldValue, _zoomLevel, nameof(ZoomLevel));
                    UpdateRichTextBoxZoom();
                }
            }
        }

        private void UpdateRichTextBoxZoom()
        {
            _richTextBox.LayoutTransform = new ScaleTransform(ZoomLevelRatio, ZoomLevelRatio);
        }



        public void RestoreZoom()
        {
            ZoomLevel = DefaultZoomLevel;
        }

        public void ZoomIn()
        {
            if (ZoomLevel < MaxZoomLevel)
            {
                ZoomLevel += 15;
            }
        }

        public void ZoomOut()
        {
            if (ZoomLevel > MinZoomLevel)
            {
                ZoomLevel -= 20;
            }
        }

        private void CutText()
        {
            _undoStack.Push(Document.Text);
            Clipboard.SetText(new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text);
            new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text = string.Empty;
        }

        private void CopyText()
        {
            Clipboard.SetText(new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text);
        }

        private void PasteText()
        {
            _undoStack.Push(Document.Text);
            new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text = Clipboard.GetText();
        }

        public void UndoText()
        {
            if (_undoStack.Count > 1)
            {
                _undoStack.Pop(); // Remove current state
                new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text = _undoStack.Peek(); // Revert to previous state
            }
        }

        public void ToggleBold()
        {
            var selectedText = _richTextBox.Selection;
            if (selectedText != null)
            {
                var isBold = selectedText.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold);
                selectedText.ApplyPropertyValue(TextElement.FontWeightProperty, isBold ? FontWeights.Regular : FontWeights.Bold);
            }
        }

        public void ToggleItalic()
        {
            var selectedText = _richTextBox.Selection;
            if (selectedText != null)
            {
                var isItalic = selectedText.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Italic);
                selectedText.ApplyPropertyValue(TextElement.FontStyleProperty, isItalic ? FontStyles.Normal : FontStyles.Italic);
            }
        }

        public void ToggleUnderline()
        {
            var selectedText = _richTextBox.Selection;
            if (selectedText != null)
            {
                var isUnderline = selectedText.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline);
                selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, isUnderline ? null : TextDecorations.Underline);
            }
        }

        public void ChangeTextColor()
        {
            var selectedText = _richTextBox.Selection;
            if (selectedText != null)
            {
                selectedText.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
        }

        private void OpenStyleDialog()
        {
            var fontDialog = new FontDialog();
            fontDialog.DataContext = Format;
            fontDialog.ShowDialog();
        }

        private void ToggleWrap()
        {
            if (Format.Wrap == TextWrapping.Wrap)
                Format.Wrap = TextWrapping.NoWrap;
            else
                Format.Wrap = TextWrapping.Wrap;
        }
    }
}