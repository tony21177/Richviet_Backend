using Microsoft.Extensions.Configuration;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Services
{
    public class BannerService : IBannerService
    {
        private readonly IUploadPic uploadPic;
        public IConfiguration Configuration { get; }

        public BannerService(IUploadPic uploadPic, IConfiguration configuration)
        {
            this.uploadPic = uploadPic;
            Configuration = configuration;
        }

        public async Task<List<string>> GetCaroudsels()
        {

            return await uploadPic.GetBlobList(Configuration["BannerCarousel"]);
        }
    }
}
