using Microsoft.AspNetCore.Http;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IUploadPic
    {
        Task<string> SavePic(UserArc userArc,byte imageType,IFormFile formFile);
    }
}
