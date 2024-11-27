using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.Ui.Controls;
using WPFNotepad.ViewModels;

namespace WPFNotepad
{
    public partial class NotepadWindow : FluentWindow
    {
        public NotepadWindow()
        {
            InitializeComponent();
            var viewModel = new NotepadViewModel(RootRichTextBox);
            this.DataContext = viewModel;
        }

        #region [Font settings handlers]
        public void lstFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFontFamily.SelectedItem != null)
            {
                RootRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, lstFontFamily.SelectedItem);
            }
        }

        public void lstFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFontSize.SelectedItem != null && double.TryParse(lstFontSize.SelectedItem.ToString(), out double textSize))
            {
                RootRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, textSize);
            }
        }
        #endregion
    }
}