using HtmlAgilityPack;
using Richviet.Task.vo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using Richviet.Tools.Utility;
using Microsoft.Extensions.Logging;

namespace Richviet.Task
{
    public class ArcValidationTask
    {
        private readonly ILogger logger;
        private readonly FolderHandler folderHandler;

        public ArcValidationTask(ILogger<ArcValidationTask> logger, FolderHandler folderHandler)
        {
            this.logger = logger;
            this.folderHandler = folderHandler;
        }

        public bool Validate(String arcNo,String issueDate,String expiredDate,String backCode)
        {
            
            var validationData = new ValidationData();
            validationData.ReNext = "查詢";
            
            SetSessionId(validationData);
            var verificationImgPath = DownloadVerficationImg();
            SetHiddenValue(validationData);
            var result = PostValidationData(validationData);

            return false;
        }


        private void SetHiddenValue(ValidationData validationData)
        {
            using (var client = new HttpClient())
            {
                var res = client.GetAsync("https://icinfo.immigration.gov.tw/NIL_WEB/NFCData.aspx").Result;
                var responseHtml = res.Content.ReadAsStringAsync().Result;
                var docs = new HtmlDocument();
                docs.LoadHtml(responseHtml);
                validationData.ViewState = docs.GetElementbyId("__VIEWSTATE").GetAttributeValue("value", "");
                validationData.ViewStateGenerator = docs.GetElementbyId("__VIEWSTATEGENERATOR").GetAttributeValue("value", "");
                validationData.EventValidation = docs.GetElementbyId("__EVENTVALIDATION").GetAttributeValue("value", "");
            }
            
        }

        private void SetSessionId(ValidationData validationData)
        {
            using (var client = new HttpClient())
            {
                string uri = "https://icinfo.immigration.gov.tw/NIL_WEB/ValidateCode.ashx";
                var res = client.GetAsync(uri).Result;
                var cookies = res.Headers.GetValues("Set-Cookie");
                foreach (var cookie in cookies)
                {
                    if (cookie.Contains("ASP.NET_SessionId"))
                    {
                        validationData.SessionId = cookie.Split(new Char[] { '=', ';' })[1];
                        break;
                    }
                }
            }
        }

        private void SetVerificationCode(HttpClient client, ValidationData validationData)
        {

        }
        private string PostValidationData(ValidationData validationData)
        {
            var baseAddress = new Uri("https://icinfo.immigration.gov.tw");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {

                IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                    { new KeyValuePair<string, string>("IDNO", validationData.Idno) },
                    { new KeyValuePair<string, string>("APPROVE_DATE", validationData.ApproveDate) },
                    { new KeyValuePair<string, string>("BARCODE_NO", validationData.BarcodeNo) },
                    { new KeyValuePair<string, string>("TextBox1", validationData.TextBox1) },
                    { new KeyValuePair<string, string>("ReNext", validationData.ReNext) },
                    { new KeyValuePair<string, string>("__VIEWSTATE", validationData.ViewState) },
                    { new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", validationData.ViewStateGenerator) },
                    { new KeyValuePair<string, string>("__EVENTVALIDATION", validationData.EventValidation) },
                };

                cookieContainer.Add(baseAddress, new Cookie("NET_SessionId", validationData.SessionId));
                var res = client.PostAsync("/NIL_WEB/NFCData.aspx", new FormUrlEncodedContent(nameValueCollection)).Result;
                var responseHtml = res.Content.ReadAsStringAsync().Result;
                var docs = new HtmlDocument();
                docs.LoadHtml(responseHtml);
                var result = docs.GetElementbyId("lblResult").InnerText;
                return result;
            }
        }

        private string DownloadVerficationImg()
        {
            var saveToFilePath = folderHandler.CreateFolder("validation").FullName + Path.DirectorySeparatorChar + "arcValidation";
            folderHandler.SaveImageFromUri(saveToFilePath, ImageFormat.Png,"https://icinfo.immigration.gov.tw/NIL_WEB/ValidateCode.ashx");
            return saveToFilePath;
        }
        
    }
}
