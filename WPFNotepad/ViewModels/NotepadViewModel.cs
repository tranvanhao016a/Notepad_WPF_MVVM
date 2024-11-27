using System.Windows.Controls;
using System.Windows.Input;
using WPFNotepad.Models;

namespace WPFNotepad.ViewModels
{
    public class NotepadViewModel
    {
        private DocumentModel _document;
        public EditorViewModel Editor { get; set; }
        public FileViewModel File { get; set; }
        public HelpViewModel Help { get; set; }

        public ICommand BoldCommand { get; }
        public ICommand ItalicCommand { get; }
        public ICommand UnderlineCommand { get; }

        public NotepadViewModel()
        {
            _document = new DocumentModel();
            Help = new HelpViewModel();
            Editor = new EditorViewModel(_document, null);
            File = new FileViewModel(_document, null);

            BoldCommand = new RelayCommand(() => { });
            ItalicCommand = new RelayCommand(() => { });
            UnderlineCommand = new RelayCommand(() => { });
        }

        public NotepadViewModel(RichTextBox richTextBox)
        {
            _document = new DocumentModel();
            Help = new HelpViewModel();
            Editor = new EditorViewModel(_document, richTextBox);
            File = new FileViewModel(_document, richTextBox);

            BoldCommand = new RelayCommand(Editor.ToggleBold);
            ItalicCommand = new RelayCommand(Editor.ToggleItalic);
            UnderlineCommand = new RelayCommand(Editor.ToggleUnderline);
        }
    }
}