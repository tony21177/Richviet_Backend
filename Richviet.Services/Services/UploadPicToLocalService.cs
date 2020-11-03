using Microsoft.AspNetCore.Http;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Richviet.Tools.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Services
{
    public class UploadPicToLocalService : IUploadPic
    {
        private readonly FolderHandler folderHandler;

        public UploadPicToLocalService(FolderHandler folderHandler)
        {
            this.folderHandler = folderHandler;
        }

        public async Task<string> SavePic(UserArc userArc, byte imageType,IFormFile image)
        {
            String resultFileName = null;
            String mainFolder = null;
            switch (imageType)
            { 
                case 0:
                    mainFolder = "instant";
                    break;
                case 1:
                    mainFolder = "signature";
                    break;
                case 2:
                    mainFolder = "front";
                    break;
                case 3:
                    mainFolder = "back";
                    break;

            }
            resultFileName = imageType + "_" + userArc.ArcNo + "_" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            String folder = mainFolder + Path.DirectorySeparatorChar + userArc.UserId;
            DirectoryInfo directoryInfo = folderHandler.CreateFolder(folder);
            var filePath = Path.Combine(directoryInfo.FullName, resultFileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            return resultFileName;
        }
    }
}
