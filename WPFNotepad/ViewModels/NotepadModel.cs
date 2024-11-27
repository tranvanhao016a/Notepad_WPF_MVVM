using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFNotepad.Models;

namespace WPFNotepad.ViewModels
{
    public class NotepadModel
    {
        //Document that is saved, loaded and hold editor text
        private DocumentModel _document;
        //Manages user input for document and format styles
        public EditorViewModel Editor { get; set; }
        //Manages saving and loading text files
        public FileViewModel File { get; set; }
        //Manage help dialog
        public HelpViewModel Help { get; set; }

        public NotepadModel()
        {
            _document = new DocumentModel();
            Help = new HelpViewModel();
            Editor = new EditorViewModel(_document);
            File = new FileViewModel(_document);
        }
    }
}
