using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;
using Newtonsoft.Json;
using ZhiHu.Photo.Common.Interfaces;
using ZhiHu.Photo.Common.Models;
using ZhiHu.Photo.Models;

namespace ZhiHu.Photo.ViewModels
{
    [ObservableObject]
    public partial class ShellViewModel
    {
        private static string Url = "https://localhost:5001";
        public ObservableCollection<AnswerInfo> Answers { get; set; } = new();

        [ObservableProperty] private string _json;

        [RelayCommand]
        private async Task Query()
        {
            Answers.Clear();
            var url = $"{Url}/api/Answer/GetAll?PageIndex=1&PageSize=30&Search=";
            var client = new HttpClient();
            Json = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<ApiResponse<PagedList<AnswerInfo>>>(Json);
            if (result.Status)
            {
                foreach (var answer in result.Data!.Items)
                {
                    Answers.Add(answer);
                }
            }
        }

    }
}