using Microsoft.AspNetCore.Http;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Richviet.Tools.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Richviet.Services.Services
{
    public class UploadPicToLocalService : IUploadPic
    {
        private readonly FolderHandler folderHandler;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string workingRootPath;



        public UploadPicToLocalService(FolderHandler folderHandler, IWebHostEnvironment webHostEnvironment)
        {
            this.folderHandler = folderHandler;
            this.webHostEnvironment = webHostEnvironment;
            this.workingRootPath = webHostEnvironment.ContentRootPath;
        }

        public async Task<string> SavePic(UserArc userArc, byte imageType,IFormFile image)
        {
            string mainFolder = imageType switch
            {
                0 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                1 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                2 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                3 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                _ => PictureTypeEnum.Other.ToString().ToLower(),
            };
            string resultFileName = imageType + "_" + userArc.ArcNo + "_" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string folder = mainFolder + Path.DirectorySeparatorChar + userArc.UserId;
            DirectoryInfo directoryInfo = folderHandler.CreateFolder(workingRootPath, folder);
            var filePath = Path.Combine(directoryInfo.FullName, resultFileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await image.CopyToAsync(stream);
            }
            return resultFileName;
        }

        public string GetPictureAbsolutePath(UserArc userArc,byte imageType,string imageFileName)
        {
            string mainFolder = imageType switch
            {
                0 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                1 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                2 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                3 => ((PictureTypeEnum)imageType).ToString().ToLower(),
                _ => PictureTypeEnum.Other.ToString().ToLower(),
            };
            string folder =  mainFolder + Path.DirectorySeparatorChar + userArc.UserId;
            DirectoryInfo directoryInfo = folderHandler.CreateFolder(workingRootPath, folder);
            var filePath = Path.Combine(directoryInfo.FullName, imageFileName);
            return filePath;
        }

        public bool CheckUploadFileExistence(UserArc userArc, PictureTypeEnum typeEnum, String fileName)
        {
            String folder = typeEnum.ToString().ToLower() + Path.DirectorySeparatorChar + userArc.UserId;
            var filePath = Path.Combine(folder, fileName);
            return folderHandler.IsFileExists(workingRootPath,filePath);
        }
    }
}
