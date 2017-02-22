using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using Leaf.Model;
using Newtonsoft.Json.Linq;
using GalaSoft.MvvmLight.Command;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace Leaf.ViewModel
{
    class InsertModel : ViewModelBase
    {
        private int singlenum = 0;
        private int gapnum = 0;
        private string _json;
        public string Json
        {
            get { return _json; }
            set { Set(ref _json, value); }
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
        private void Insert()
        {
            try
            {
                if (Json == null)
                    return;
                JObject _jsonobject = JObject.Parse(Json);
                JArray _singlearray = JArray.Parse(_jsonobject["Single"].ToString());
                singlenum = 0;
                gapnum = 0;
                using (var mydb = new MyDBContext())
                {
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
                        mydb.SingleChoices.Add(model);
                        singlenum += 1;

                    }
                    JArray _gaplist = JArray.Parse(_jsonobject["Gap"].ToString());

                    foreach (var token in _gaplist)
                    {
                        GapFilling model = new GapFilling();
                        model.Answer = token["Answer"].ToString();
                        model.Stems = token["Stems"].ToString();
                         model.Level = Convert.ToInt32(token["Level"].ToString());
                        model.Type = token["Type"].ToString();
                        model.Subject = token["Subject"].ToString();
                        mydb.GapFillings.Add(model);
                        gapnum += 1;
                    }

                    mydb.SaveChanges();

                    int[] num = new int[2] { singlenum, gapnum };
                    singlenum = 0;
                    gapnum = 0;
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<int[]>(num, "InsertYes");
                    ViewModelLocator.QuestionList.Init();
                    Json = "";
                }
            }
            catch(Exception e)
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
            _openFile.FileTypeFilter.Add(".json");
            _openFile.FileTypeFilter.Add(".txt");
            try
            {
                StorageFile file = await _openFile.PickSingleFileAsync();
                if (file != null)
                {
                    Json = await FileIO.ReadTextAsync(file);
                }
            }
            catch(Exception e)
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string>(e.Message, "Exception");
            }
            
        }

        public InsertModel()
        {
            InsertCommand = new RelayCommand(Insert);
            OpenCommand = new RelayCommand(openfile);
        }
    }
}
