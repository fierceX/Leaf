using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Leaf.Model
{
    class FileService
    {
        private string _text = null;
        public string Text
        {
            get
            {
                return _text;
            }
        }

        public async void OpenFile()
        {
            var _openFile = new FileOpenPicker();
            _openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            _openFile.ViewMode = PickerViewMode.List;
            _openFile.FileTypeFilter.Add(".json");
            _openFile.FileTypeFilter.Add(".txt");
            StorageFile file = await _openFile.PickSingleFileAsync();
            _text = await FileIO.ReadTextAsync(file);
        }

    }
}
