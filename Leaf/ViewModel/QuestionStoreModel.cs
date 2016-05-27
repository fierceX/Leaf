﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaf.Model;
using Windows.Web.Http.Headers;
using Windows.Data.Json;
using Windows.Web.Http;
using Leaf.SQLite;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Leaf.ViewModel
{
    class QuestionStoreModel : ViewModelBase
    {

        private int singlenum = 0;
        private int gapnum = 0;
        /// <summary>
        /// 题目列表
        /// </summary>
        private List<QuestionView> _questionlist = new List<QuestionView>();
        public List<QuestionView> QuestionList
        {
            get { return _questionlist; }
            set { Set(ref _questionlist, value); }
        }

        /// <summary>
        /// 选中题目索引
        /// </summary>
        private int _questionIndex;
        public int QuestionIndex
        {
            get { return _questionIndex; }
            set { Set(ref _questionIndex, value); }
        }


        public ICommand Download { get; set; }
        private async void download()
        {
            var sdb = new DbSingleService();
            var gdb = new DbGapService();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(new Uri("http://localhost:2832/api/SingleChoices?subject="+QuestionList[QuestionIndex].Subject));
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            JsonArray arrays = JsonArray.Parse(result);
            for (int i = 0; i < arrays.Count; i++)
            {
                JsonObject obj = arrays.GetObjectAt((uint)i);
                SingleChoice model = new SingleChoice();
                model.Stems = obj.GetNamedString("Stems");
                model.Subject = obj.GetNamedString("subject");
                model.Answer = obj.GetNamedString("Answer");
                model.Choices1 = obj.GetNamedString("Choices1");
                model.Choices2 = obj.GetNamedString("Choices2");
                model.Choices3 = obj.GetNamedString("Choices3");
                model.Level = (int) obj.GetNamedNumber("level");
                model.Type = obj.GetNamedString("type");
                int n = sdb.Insert(model);
                if (n > 0)
                    singlenum += n;
            }
            response = await httpClient.GetAsync(new Uri("http://localhost:2832/api/GapFillings?subject=" + QuestionList[QuestionIndex].Subject));
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadAsStringAsync();
            arrays = JsonArray.Parse(result);
            for (int i = 0; i < arrays.Count; i++)
            {
                JsonObject obj = arrays.GetObjectAt((uint)i);
                GapFilling model = new GapFilling();
                model.Stems = obj.GetNamedString("Stems");
                model.Subject = obj.GetNamedString("subject");
                model.Answer = obj.GetNamedString("Answer");
                model.Level = (int)obj.GetNamedNumber("level");
                model.Type = obj.GetNamedString("type");
                int n = gdb.Insert(model);
                if (n > 0)
                    gapnum += n;
            }
            int[] num = new int[2] { singlenum, gapnum };
            singlenum = 0;
            gapnum = 0;
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<int[]>(num, "DownloadYes");

        }

        /// <summary>
        /// 读取数据
        /// </summary>
        private async void ReadData()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(new Uri("http://localhost:2832/api/Subjects"));
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            JsonArray arrays = JsonArray.Parse(result);
            for (int i = 0; i < arrays.Count; i++)
            {
                JsonObject obj = arrays.GetObjectAt((uint)i);
                QuestionView model = new QuestionView();
                model.GapNum = (int)obj.GetNamedNumber("gapnum");
                model.SingleNum = (int)obj.GetNamedNumber("singlenum");
                model.Subject = obj.GetNamedString("subject");
                model.Type = obj.GetNamedString("type");
                model.Level = (int)obj.GetNamedNumber("level");
                switch (model.Level)
                {
                    case 1:
                        {
                            model.Color = "#00FF00";
                        }
                        break;
                    case 2:
                        {
                            model.Color = "#008B00";
                        }
                        break;
                    case 3:
                        {
                            model.Color = "#CDCD00";
                        }
                        break;
                    default: break;
                }
                QuestionList.Add(model);
            }

        }

        public void Init()
        {
            ReadData();
            Download = new RelayCommand(download);
        }

        public QuestionStoreModel()
        {
            Init();
        }
    }
}