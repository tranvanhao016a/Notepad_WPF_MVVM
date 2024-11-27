using System.Windows.Controls;
using System.Windows.Input;
using WPFNotepad.Models;

namespace WPFNotepad.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public FileViewModel File { get; }
        public EditorViewModel Editor { get; }
        public HelpViewModel Help { get; }

        public MainViewModel()
        {
            var document = new DocumentModel();
            var richTextBox = new RichTextBox(); // Assuming you have a way to pass the RichTextBox instance
            File = new FileViewModel(document, richTextBox);
            Editor = new EditorViewModel(document, richTextBox);
            Help = new HelpViewModel();
        }
    }
}