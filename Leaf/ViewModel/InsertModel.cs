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
        /// <summary>
        /// 待插入的选择题集合
        /// </summary>
        private List<SingleChoice> _singlelist;

        /// <summary>
        /// 待插入的填空题的集合
        /// </summary>
        private List<GapFilling> _gaplist;

        /// <summary>
        /// 待插入的图片压缩集合
        /// </summary>
        private List<ZipArchiveEntry> _ziplist;

        /// <summary>
        /// 单选题数量
        /// </summary>
        private int _singlenum;

        public int SingleNum
        {
            get { return _singlenum; }
            set { Set(ref _singlenum, value); }
        }

        /// <summary>
        /// 填空题数量
        /// </summary>
        private int _gapnum;

        public int GapNum
        {
            get { return _gapnum; }
            set { Set(ref _gapnum, value); }
        }

        /// <summary>
        /// 图片数量
        /// </summary>
        private int _imagenum;

        public int ImageNum
        {
            get { return _imagenum; }
            set { Set(ref _imagenum, value); }
        }

        /// <summary>
        /// 图片占用大小
        /// </summary>
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

        /// <summary>
        /// 插入习题和图片
        /// </summary>
        private async void InsertAsync()
        {
            try
            {
                // 找到程序自己的数据文件夹
                StorageFolder state = ApplicationData.Current.LocalFolder;
                // 定义一个不重复的文件夹名，按照时间来不会重复
                string name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                // 创建文件夹用来存放图片
                StorageFolder jpg = await state.CreateFolderAsync(name);
                // 循环遍历图片压缩包，解压到指定文件夹
                foreach (var x in _ziplist)
                {
                    x.ExtractToFile(Path.Combine(jpg.Path, x.FullName));
                }
                // 插入习题
                using (var mydb = new MyDBContext())
                {
                    // 循环遍历单选题和填空题，设置图片路径
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
                    // 保存至数据库并且清空暂存数据
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

        /// <summary>
        /// 打开待插入题库
        /// </summary>
        private async void openfile()
        {
            // 创建一个文件拾取器
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
                // 读取选取的文件
                StorageFile file = await _openFile.PickSingleFileAsync();
                if (file != null)
                {
                    // 读取压缩包
                    Stream stream = await file.OpenStreamForReadAsync();
                    ZipArchive zip = new ZipArchive(stream);

                    // 读取压缩包里的data.json文件，也就是题库内容文件
                    ZipArchiveEntry z = zip.GetEntry("data.json");
                    // 获取程序临时文件存放路径
                    StorageFolder cache = ApplicationData.Current.LocalCacheFolder;
                    // 解压缩data.json
                    z.ExtractToFile(Path.Combine(cache.Path, "data.json"), true);
                    // 读取data.json
                    StorageFile f = await cache.GetFileAsync("data.json");
                    // 解析json里的内容
                    Resolving(await FileIO.ReadTextAsync(f));

                    //遍历压缩包里的内容，统计图片大小和数量，并保存至带解压图片集合里
                    foreach (ZipArchiveEntry x in zip.Entries)
                    {
                        if (x.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || x.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            size += Math.Ceiling((double)x.Length / 1024);
                            ImageNum += 1;
                            _ziplist.Add(x);
                        }
                    }
                    // 计算待解压图片大小
                    StorageSize = (Math.Ceiling(size / 1024 * 100) / 100).ToString() + " Mb";
                }
            }
            catch (Exception e)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(e.Message, "Exception");
            }
        }

        /// <summary>
        /// 分析题库内容
        /// </summary>
        /// <param name="_json">json字符串</param>
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

                // 创建json读取对象
                JObject _jsonobject = JObject.Parse(_json);
                // 读取选择题
                JArray _singlearray = JArray.Parse(_jsonobject["Single"].ToString());
                SingleNum = 0;
                GapNum = 0;

                // 遍历读取选择题
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
                // 读取填空题
                JArray _gaparray = JArray.Parse(_jsonobject["Gap"].ToString());
                // 遍历读取填空题
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