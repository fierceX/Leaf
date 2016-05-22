using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Leaf.Model;
using Newtonsoft.Json.Linq;
using GalaSoft.MvvmLight.Command;
using Leaf.SQLite;

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

        public ICommand InsertCommand { get; set; }
        private void Insert()
        {
            JObject _jsonobject = JObject.Parse(Json);
            JArray _singlearray = JArray.Parse(_jsonobject["Single"].ToString());
            foreach(var token in _singlearray)
            {
                var db = new DbSingleService();
                SingleChoice model = new SingleChoice();
                model.Answer = token["Answer"].ToString();
                model.Stems = token["Stems"].ToString();
                model.Choices1 = token["choices"][0].ToString();
                model.Choices2 = token["choices"][1].ToString();
                model.Choices3 = token["choices"][2].ToString();
                model.Level = Convert.ToInt32(token["Level"].ToString());
                model.Type = token["Type"].ToString();
                int i = db.Insert(model);
                if (i > 0)
                    singlenum += i;
                
            }
            JArray _gaplist = JArray.Parse(_jsonobject["Gap"].ToString());
            foreach(var token in _gaplist)
            {
                var db = new DbGapService();
                GapFilling model = new GapFilling();
                model.Answer = token["Answer"].ToString();
                model.Stems = token["Stems"].ToString();
                model.Level = Convert.ToInt32(token["Level"].ToString());
                model.Type = token["Type"].ToString();
                int i = db.Insert(model);
                if (i > 0)
                    gapnum += i;
            }
            int[] num = new int[2] { singlenum, gapnum };
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<int[]>(num, "InsertYes");

        }

        public InsertModel()
        {
            InsertCommand = new RelayCommand(Insert);
        }
    }
}
