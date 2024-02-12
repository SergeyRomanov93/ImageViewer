using ImageViewer.Command;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ImageViewer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private static readonly string[] _supportedExtensions = new[] 
        { 
            ".jpg", 
            ".png", 
            ".bmp", 
            ".jpeg", 
            ".tiff" 
        };

        private string _photosDirectory;
        private Dictionary<string, string> _photos;
        private KeyValuePair<string, string> _selectedPhoto;
        private float _factor;
        private float _vertical;
        private float _horizontal;
        private float _vMax;
        private float _hMax;
        private BitmapImage _image;

        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public float HMax
        {
            get => _vMax;
            set => SetProperty(ref _vMax, value);
        }
        public float VMax
        {
            get => _hMax;
            set => SetProperty(ref _hMax, value);
        }
        public float Vertical
        {
            get => _vertical;
            set => SetProperty(ref _vertical, value);
        }
        public float Horizontal
        {
            get => _horizontal;
            set => SetProperty(ref _horizontal, value);
        }

        public float Factor
        {
            get => _factor;
            set => SetProperty(ref _factor, value);
        }

        public string PhotosDirectory
        {
            get => _photosDirectory;
            set => SetProperty(ref _photosDirectory, value);
        }

        public RelayCommand SearchButtonCommand { get; set; }

        public Dictionary<string, string> SearchResults
        {
            get => _photos;
            set => SetProperty(ref _photos, value);
        }

        public KeyValuePair<string, string> SelectedPhoto
        {
            get => _selectedPhoto;
            set
            {
                SetProperty(ref _selectedPhoto, value);

                var filePath = Path.Combine(PhotosDirectory, _selectedPhoto.Key);
                Image = new BitmapImage(new Uri(filePath));

                HMax = (int)Image.Width / 2;
                VMax = (int)Image.Height / 2;
            }
        }

        public MainWindowViewModel()
        {
            Factor = 1;
            SearchButtonCommand = new RelayCommand(o => Search());
        }

        private void Search()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                MessageBox.Show("Папка не выбрана!");
                return;
            }

            PhotosDirectory = folderBrowserDialog.SelectedPath;
            SearchResults = GetFiles(PhotosDirectory);
        }

        private static Dictionary<string, string> GetFiles(string photosDirectory) => Directory
            .EnumerateFiles(photosDirectory)
            .Select(fn => (fn, ext: Path.GetExtension(fn)))
            .Where(x => _supportedExtensions.Contains(x.ext))
            .ToDictionary(x => x.fn, x => Path.GetFileNameWithoutExtension(x.fn));
    }
}