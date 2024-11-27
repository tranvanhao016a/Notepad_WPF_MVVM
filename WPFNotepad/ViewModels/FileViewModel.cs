using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Controls;
using WPFNotepad.Models;
using System.Windows.Media;

namespace WPFNotepad.ViewModels
{
    public class FileViewModel
    {
        public DocumentModel Document { get; private set; }
        private readonly RichTextBox _richTextBox;

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand OpenNewWindows { get; }
        public ICommand OpenCommand { get; }
        public ICommand ExitCommand { get; }

      

        public FileViewModel(DocumentModel document, RichTextBox richTextBox)
        {
            Document = document;
            _richTextBox = richTextBox;
            NewCommand = new RelayCommand(NewFile);
            SaveCommand = new RelayCommand(SaveFile, () => !Document.isEmpty);
            SaveAsCommand = new RelayCommand(SaveFileAs);
            OpenCommand = new RelayCommand(OpenFile);
            OpenNewWindows = new RelayCommand(OpenNewWindow);
            ExitCommand = new RelayCommand(() => Environment.Exit(0));
        }

        public void NewFile()
        {
            Document.FileName = string.Empty;
            Document.FilePath = string.Empty;
            Document.Text = string.Empty;
            _richTextBox.Document.Blocks.Clear();
        }

        private void SaveFile()
        {
            TextRange range = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);
            using (FileStream fileStream = new FileStream(Document.FilePath, FileMode.Create))
            {
                if (Path.GetExtension(Document.FilePath).ToLower() == ".rtf")
                {
                    range.Save(fileStream, DataFormats.Rtf);
                }
                else
                {
                    range.Save(fileStream, DataFormats.Text);
                }
            }
        }

        private void SaveFileAs()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|Text File (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                DockFile(saveFileDialog);
                SaveFile();
            }
        }

        public void OpenNewWindow()
        {
            var newWindow = new NotepadWindow();
            newWindow.Show();
        }

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                DockFile(openFileDialog);
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    TextRange range = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);
                    if (Path.GetExtension(openFileDialog.FileName).ToLower() == ".rtf")
                    {
                        range.Load(fileStream, DataFormats.Rtf);
                    }
                    else
                    {
                        range.Load(fileStream, DataFormats.Text);
                    }
                }
                Document.Text = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd).Text;
               
            }
        }

      
        

        private void DockFile<T>(T dialog) where T : FileDialog
        {
            Document.FilePath = dialog.FileName;
            Document.FileName = dialog.SafeFileName;
        }
    }
}