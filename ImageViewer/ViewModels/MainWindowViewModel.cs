using ImageViewer.Command;
using Prism.Mvvm;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ImageViewer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _photosDirectory;
        private List<string> _photos;

        public string PhotosDirectory
        {
            get { return _photosDirectory; }
            set
            {
                _photosDirectory = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SearchButtonCommand { get; set; }

        public List<string> SearchResults
        {
            get => _photos;
            set
            {
                _photos = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            SearchButtonCommand = new RelayCommand(o => Search());
        }

        private void Search()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            PhotosDirectory = folderBrowserDialog.SelectedPath;
            SearchResults = GetFiles(PhotosDirectory);
        }

        private List<string> GetFiles(string photosDirectory)
        {
            List<string> files = new List<string>();

            foreach (var file in Directory.EnumerateFiles(photosDirectory, "*.jpg"))
            {
                files.Add(Path.GetFileNameWithoutExtension(file));
            }

            return files;
        }
    }
}
