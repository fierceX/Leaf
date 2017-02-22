using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Leaf.Model
{
    /// <summary>
    /// 文件服务类，用于导入离线题库时打开文件用
    /// </summary>
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

        /// <summary>
        /// 打开文件
        /// </summary>
        public async void OpenFile()
        {
            //定义一个文件拾取器
            var _openFile = new FileOpenPicker();
            //设置文件拾取器打开文件类型
            _openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            _openFile.ViewMode = PickerViewMode.List;
            _openFile.FileTypeFilter.Add(".json");
            _openFile.FileTypeFilter.Add(".txt");
            StorageFile file = await _openFile.PickSingleFileAsync();
            _text = await FileIO.ReadTextAsync(file);
        }

    }
}
