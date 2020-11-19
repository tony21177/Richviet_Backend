using Microsoft.AspNetCore.Http;
using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IUploadPic
    {
        Task<string> SavePic(UserArc userArc,byte imageType,IFormFile formFile);

        string GetPictureAbsolutePath(UserArc userArc, byte imageType, string imageFileName);

        bool CheckUploadFileExistence(UserArc userArc, PictureTypeEnum type, String fileName);
    }
}
