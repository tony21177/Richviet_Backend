using Microsoft.AspNetCore.Http;
using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Richviet.Services.Contracts
{
    public interface IUploadPic
    {
        Task<string> SavePic(UserArc userArc,byte imageType,IFormFile formFile);

        Task<Stream> LoadImage(UserArc userArc, byte imageType, string imageFileName);

        Task<bool> CheckUploadFileExistence(UserArc userArc, PictureTypeEnum type, String fileName);
    }
}
