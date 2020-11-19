using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using Richviet.Tools.Utility;
using Microsoft.Extensions.Logging;
using Anticaptcha.Helper;
using Anticaptcha.Api;
using Microsoft.Extensions.Configuration;
using Richviet.BackgroudTask.Arc.Vo;
using System.Threading.Tasks;
using Richviet.BackgroudTask.vo;
using static Anticaptcha.Api.ImageToText;

namespace Richviet.BackgroudTask.Arc
{
    public class ArcValidationTask
    {
        private readonly ILogger logger;
        private readonly FolderHandler folderHandler;
        private readonly IConfiguration _configuration;
       

        public ArcValidationTask(ILogger<ArcValidationTask> logger, FolderHandler folderHandler, IConfiguration configuration)
        {
            this.logger = logger;
            this.folderHandler = folderHandler;
            this._configuration = configuration;
        }

        public async Task<ArcValidationResult> Validate(string workingRootPath,String arcNo,String issueDate,String expiredDate,String backCode)
        {
            try{
                
                var validationData = new ValidationData();
                validationData.Idno = arcNo;
                validationData.ApproveDate = issueDate;
                validationData.ExpiredDate = expiredDate;
                validationData.BarcodeNo = backCode;
                validationData.ReNext = "查詢";

                await SetSessionId(validationData);
                var verificationImgPath = DownloadVerficationImg(workingRootPath,validationData);
                await SetVerificationCode(validationData, verificationImgPath, _configuration["Anticaptcha_client_key"]);
                await SetHiddenValue(validationData);
                var result = await PostValidationData(validationData);
                logger.LogInformation("ARC result : {result}", result);
                while (true)
                {
                    switch (result)
                    {
                        case "資料相符":
                            return new ArcValidationResult()
                            {
                                IsSuccessful = true,
                                Result = result
                            };
                        case "資料不符":
                            return new ArcValidationResult()
                            {
                                IsSuccessful = false,
                                Result = result
                            };
                        case "驗證碼錯誤":
                        case "請重新輸入驗證碼":
                            verificationImgPath = DownloadVerficationImg(workingRootPath,validationData);
                            await SetVerificationCode(validationData, verificationImgPath, _configuration["Anticaptcha_client_key"]);
                            result = await PostValidationData(validationData);
                            logger.LogInformation("retry Anticaptcha result : {result}", new string[] { result });
                            continue;
                        default:
                            return new ArcValidationResult()
                            {
                                IsSuccessful = false
                            };
                    }
                }
                
                
            }
            catch (Exception exception)
            {
                logger.LogDebug("Anticaptcha fail,{exception}", new Exception[] { exception });
                return new ArcValidationResult()
                {
                    IsSuccessful = false,
                    Result = exception.Message
                };
            }
            

        }


        private async System.Threading.Tasks.Task SetHiddenValue(ValidationData validationData)
        {
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync("https://icinfo.immigration.gov.tw/NIL_WEB/NFCData.aspx");
                var responseHtml = res.Content.ReadAsStringAsync().Result;
                var docs = new HtmlDocument();
                docs.LoadHtml(responseHtml);
                validationData.ViewState = docs.GetElementbyId("__VIEWSTATE").GetAttributeValue("value", "");
                validationData.ViewStateGenerator = docs.GetElementbyId("__VIEWSTATEGENERATOR").GetAttributeValue("value", "");
                validationData.EventValidation = docs.GetElementbyId("__EVENTVALIDATION").GetAttributeValue("value", "");
            }
            
        }

        private async System.Threading.Tasks.Task SetSessionId(ValidationData validationData)
        {
            using (var client = new HttpClient())
            {
                string uri = "https://icinfo.immigration.gov.tw/NIL_WEB/ValidateCode.ashx";
                var res = await client.GetAsync(uri);
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

        
        private async Task<string> PostValidationData(ValidationData validationData)
        {
            logger.LogInformation("validationData:" + validationData.ToString());
            var baseAddress = new Uri("https://icinfo.immigration.gov.tw");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {

                IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                    { new KeyValuePair<string, string>("IDNO", validationData.Idno) },
                    { new KeyValuePair<string, string>("APPROVE_DATE", validationData.ApproveDate) },
                    { new KeyValuePair<string, string>("END_STAY_PERIOD", validationData.ExpiredDate) },
                    { new KeyValuePair<string, string>("BARCODE_NO", validationData.BarcodeNo) },
                    { new KeyValuePair<string, string>("TextBox1", validationData.TextBox1) },
                    { new KeyValuePair<string, string>("ReNext", validationData.ReNext) },
                    { new KeyValuePair<string, string>("__VIEWSTATE", validationData.ViewState) },
                    { new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", validationData.ViewStateGenerator) },
                    { new KeyValuePair<string, string>("__EVENTVALIDATION", validationData.EventValidation) },
                };
                

                cookieContainer.Add(baseAddress, new Cookie("ASP.NET_SessionId", validationData.SessionId));
                var res = await client.PostAsync("/NIL_WEB/NFCData.aspx", new FormUrlEncodedContent(nameValueCollection));
                var responseHtml = res.Content.ReadAsStringAsync().Result;
                var docs = new HtmlDocument();
                docs.LoadHtml(responseHtml);
                var result = docs.GetElementbyId("lblResult").InnerText;
                return result;
            }
        }

        private string DownloadVerficationImg(string workingRootPath,ValidationData validationData)
        {
            var saveToFilePath = folderHandler.CreateFolder(workingRootPath,"validation").FullName + Path.DirectorySeparatorChar + "arcValidation.png";
            folderHandler.SaveImageFromUri(workingRootPath,saveToFilePath, ImageFormat.Png,"https://icinfo.immigration.gov.tw/NIL_WEB/ValidateCode.ashx", "ASP.NET_SessionId", validationData.SessionId);
            return saveToFilePath;
        }

        private async System.Threading.Tasks.Task SetVerificationCode(ValidationData validationData,string picFilePath,string clientKey)
        {
            DebugHelper.VerboseMode = true;

            var api = new ImageToText(NumericOption.NumbersOnly,0)
            {
                ClientKey = clientKey,
                FilePath = picFilePath
            };

            if (!api.CreateTask())
                DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
            else if (!api.WaitForResult())
                DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
            else
            {
                var result = api.GetTaskSolution().Text;
                DebugHelper.Out("Result: " + result, DebugHelper.Type.Success);
                validationData.TextBox1 = result;
            }
            
        }
        
    }
}
