using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Leaf.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Leaf.ViewModel
{
    internal class InsertModel : ViewModelBase
    {
        private List<SingleChoice> _singlelist;
        private List<GapFilling> _gaplist;
        private List<ZipArchiveEntry> _ziplist;

        private int _singlenum;

        public int SingleNum
        {
            get { return _singlenum; }
            set { Set(ref _singlenum, value); }
        }

        private int _gapnum;

        public int GapNum
        {
            get { return _gapnum; }
            set { Set(ref _gapnum, value); }
        }

        private int _imagenum;

        public int ImageNum
        {
            get { return _imagenum; }
            set { Set(ref _imagenum, value); }
        }

        private string _storagesize;

        public string StorageSize
        {
            get { return _storagesize; }
            set { Set(ref _storagesize, value); }
        }

        /// <summary>
        /// 添加按钮状态
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                if (ViewModelLocator.User.Admin == 1)
                    return true;
                else
                    return false;
            }
        }

        public ICommand InsertCommand { get; set; }

        private async void InsertAsync()
        {
            try
            {
                StorageFolder state = ApplicationData.Current.LocalFolder;
                string name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                StorageFolder jpg = await state.CreateFolderAsync(name);
                foreach (var x in _ziplist)
                {
                    x.ExtractToFile(Path.Combine(jpg.Path, x.FullName));
                }
                using (var mydb = new MyDBContext())
                {
                    if (_singlelist != null && _singlelist.Count > 0)
                    {
                        foreach (var x in _singlelist)
                        {
                            x.ImgPath = Path.Combine(jpg.Path, x.ImgPath);
                            mydb.SingleChoices.Add(x);
                        }
                    }
                    if (_gaplist != null && _gaplist.Count > 0)
                    {
                        foreach (var x in _gaplist)
                        {
                            x.ImgPath = Path.Combine(jpg.Path, x.ImgPath);
                            mydb.GapFillings.Add(x);
                        }
                    }
                    if (mydb.SaveChanges() > 0)
                    {
                        string[] num = new string[4] { SingleNum.ToString(), GapNum.ToString(), ImageNum.ToString(), StorageSize };
                        SingleNum = 0;
                        GapNum = 0;
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string[]>(num, "InsertYes");
                        ViewModelLocator.QuestionList.Init();
                        _singlelist.Clear();
                        _gaplist.Clear();
                        StorageSize = "";
                        ImageNum = 0;
                    }
                }
            }
            catch (Exception e)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(e.Message, "Exception");
                //throw e;
            }
        }

        public ICommand OpenCommand { get; set; }

        private async void openfile()
        {
            var _openFile = new FileOpenPicker();
            _openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            _openFile.ViewMode = PickerViewMode.List;
            _openFile.FileTypeFilter.Add(".zip");
            double size = 0;
            ImageNum = 0;
            GapNum = 0;
            SingleNum = 0;
            StorageSize = "";

            if (_singlelist == null)
                _singlelist = new List<SingleChoice>();
            else
                _singlelist.Clear();

            if (this._gaplist == null)
                this._gaplist = new List<GapFilling>();
            else
                this._gaplist.Clear();
            if (_ziplist == null)
                _ziplist = new List<ZipArchiveEntry>();
            else
                _ziplist.Clear();
            try
            {
                StorageFile file = await _openFile.PickSingleFileAsync();
                if (file != null)
                {
                    Stream stream = await file.OpenStreamForReadAsync();
                    ZipArchive zip = new ZipArchive(stream);
                    ZipArchiveEntry z = zip.GetEntry("data.json");

                    StorageFolder cache = ApplicationData.Current.LocalCacheFolder;
                    z.ExtractToFile(Path.Combine(cache.Path, "data.json"), true);
                    StorageFile f = await cache.GetFileAsync("data.json");

                    foreach (ZipArchiveEntry x in zip.Entries)
                    {
                        if (x.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || x.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            size += Math.Ceiling((double)x.Length / 1024);
                            ImageNum += 1;
                            _ziplist.Add(x);
                        }
                    }
                    StorageSize = (Math.Ceiling(size / 1024 * 100) / 100).ToString() + " Mb";
                    string _json = await FileIO.ReadTextAsync(f);
                    Resolving(_json);
                }
            }
            catch (Exception e)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(e.Message, "Exception");
            }
        }

        private void Resolving(string _json)
        {
            try
            {
                if (_json == null)
                    return;
                if (_singlelist == null)
                    _singlelist = new List<SingleChoice>();
                else
                    _singlelist.Clear();

                if (this._gaplist == null)
                    this._gaplist = new List<GapFilling>();
                else
                    this._gaplist.Clear();

                JObject _jsonobject = JObject.Parse(_json);
                JArray _singlearray = JArray.Parse(_jsonobject["Single"].ToString());
                SingleNum = 0;
                GapNum = 0;

                foreach (var token in _singlearray)
                {
                    SingleChoice model = new SingleChoice();
                    model.Answer = token["Answer"].ToString();
                    model.Stems = token["Stems"].ToString();
                    model.Choices1 = token["choices"][0].ToString();
                    model.Choices2 = token["choices"][1].ToString();
                    model.Choices3 = token["choices"][2].ToString();
                    model.Level = Convert.ToInt32(token["Level"].ToString());
                    model.Type = token["Type"].ToString();
                    model.Subject = token["Subject"].ToString();
                    model.ImgPath = token["Image"].ToString();
                    _singlelist.Add(model);
                    SingleNum += 1;
                }
                JArray _gaparray = JArray.Parse(_jsonobject["Gap"].ToString());

                foreach (var token in _gaparray)
                {
                    GapFilling model = new GapFilling();
                    model.Answer = token["Answer"].ToString();
                    model.Stems = token["Stems"].ToString();
                    model.Level = Convert.ToInt32(token["Level"].ToString());
                    model.Type = token["Type"].ToString();
                    model.Subject = token["Subject"].ToString();
                    model.ImgPath = token["Image"].ToString();
                    _gaplist.Add(model);
                    GapNum += 1;
                }
            }
            catch (Exception e)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(e.Message, "Exception");
                //throw e;
            }
        }

        public InsertModel()
        {
            InsertCommand = new RelayCommand(InsertAsync);
            OpenCommand = new RelayCommand(openfile);
        }
    }
}