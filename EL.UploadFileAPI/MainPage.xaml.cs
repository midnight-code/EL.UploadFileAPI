using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Runtime.InteropServices.ComTypes;
using Xamarin.Forms.PlatformConfiguration;

namespace EL.UploadFileAPI
{
    public partial class MainPage : ContentPage
    {
        public int id = 35;
        public MainPage()
        {
            InitializeComponent();
        }
        private async void btnFileUpload_Clicked(object sender, EventArgs e)
        {
            var file = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;

           using (var multipartFormContent = new MultipartFormDataContent())
            {
                var httpClient = new HttpClient();
                //Load the file and set the file's Content-Type header
                var fileStreamContent = new StreamContent(File.OpenRead(file.FullPath));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: file.FileName);

                //Send it
                var response = await httpClient.PostAsync("https://api.itiss.ru/uploadedocument", multipartFormContent);
                response.EnsureSuccessStatusCode();
                var r = await response.Content.ReadFromJsonAsync<int>();
                id = r;
                if (response.IsSuccessStatusCode)
                    
                lblStatus.Text = r.ToString();



            }
        }

        private async void btnFileDownload_Clicked(object sender, EventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(51), Encoding.UTF8, "application/json");
                using (var response = httpClient.PostAsync($"https://api.itiss.ru/DownloadDocument/1", content).Result)
                {
                    if (((int)response.StatusCode) == 200)
                    {
                        var ms = await response.Content.ReadAsStreamAsync();
                        var dyt= await response.Content.ReadAsByteArrayAsync();
                        ms.Seek(0, SeekOrigin.Begin);
                        imgLoad.Source = ImageSource.FromStream(() => new MemoryStream(dyt));
                    }
                }
            }
        }
    }
}
