using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Richviet.Tools.Utility
{
    public class FolderHandler
    {
        private readonly IConfiguration _configuration;

        public FolderHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DirectoryInfo CreateFolder(string folderName)
        {
            return Directory.CreateDirectory(".."+Path.DirectorySeparatorChar + _configuration["StoredFilesPath"]+ Path.DirectorySeparatorChar + folderName);
        }

        public bool IsFileExists(string filePath)
        {
            return File.Exists(".." + Path.DirectorySeparatorChar + _configuration["StoredFilesPath"] + Path.DirectorySeparatorChar + filePath);
        }

        public void SaveImageFromUri(string fileNamePath, ImageFormat format, String uri,String cookieName,String cookieValue)
        {
            CookieAwareWebClient client = new CookieAwareWebClient();
            if(cookieName!=null && cookieValue != null)
            {
                client.CookieContainer.Add(new Uri(uri),new Cookie(cookieName, cookieValue));
            }
            Stream stream = client.OpenRead(uri);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                DirectoryInfo directoryInfo = CreateFolder("validation");
                bitmap.Save(fileNamePath, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        private class CookieAwareWebClient : WebClient
        {
            public CookieContainer CookieContainer { get; set; } = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest request = base.GetWebRequest(uri);
                if (request is HttpWebRequest)
                {
                    (request as HttpWebRequest).CookieContainer = CookieContainer;
                }
                return request;
            }
        }

    }
}
