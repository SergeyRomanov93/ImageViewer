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
        public string PhotosDirectory { get => _photosDirectory; set => _photosDirectory = value; }

        public RelayCommand SearchButtonCommand { get; set; }

        public List<string> SearchResults { get; }

        public MainWindowViewModel()
        {
            SearchButtonCommand = new RelayCommand(o => Search(ref _photosDirectory));
        }

        private void Search(ref string photosDirectory)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            photosDirectory = folderBrowserDialog.SelectedPath;
        }

        private List<string> GetFiles()
        {
            List<string> files = new List<string>();

            foreach (var file in Directory.EnumerateFiles(PhotosDirectory, $"*.jpeg|.png|.tiff"))
            {
                files.Add(Path.GetFileNameWithoutExtension(file));
            }

            return files;
        }
    }
}
