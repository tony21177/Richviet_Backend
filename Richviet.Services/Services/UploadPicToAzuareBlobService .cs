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
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace Richviet.Services.Services
{
    public class UploadPicToAzuareBlobService : IUploadPic
    {
        public IConfiguration Configuration { get; }
        string accessKey = string.Empty;


        public UploadPicToAzuareBlobService(IConfiguration configuration)
        {
            Configuration = configuration;
            accessKey = Configuration.GetConnectionString("AccessKey");

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
            string fileFullName = folder + resultFileName;

            try
            {
                CloudBlobContainer cloudBlobContainer = await GetOrCreateCloudBlobContainer();

                if (fileFullName != null && image != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileFullName);
                    cloudBlockBlob.Properties.ContentType = image.ContentType;
                    await cloudBlockBlob.UploadFromStreamAsync(image.OpenReadStream(), image.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Stream> LoadImage(UserArc userArc,byte imageType,string imageFileUri)
        {
            String [] imageUriArray = imageFileUri.Split('/');
            String imageFileName = WebUtility.UrlDecode(imageUriArray[imageUriArray.Length - 1]);
            CloudBlobContainer cloudBlobContainer = await GetOrCreateCloudBlobContainer();
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageFileName);
 
            return await cloudBlockBlob.OpenReadAsync();
        }

        public async Task<bool> CheckUploadFileExistence(UserArc userArc, PictureTypeEnum typeEnum, String imageFileUri)
        {
            String[] imageUriArray = imageFileUri.Split('/');
            String imageFileName = WebUtility.UrlDecode(imageUriArray[imageUriArray.Length - 1]);
            CloudBlobContainer cloudBlobContainer = await GetOrCreateCloudBlobContainer();
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageFileName);
            bool isExist = await cloudBlockBlob.ExistsAsync();
            return isExist;
        }

        private async Task<CloudBlobContainer> GetOrCreateCloudBlobContainer()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = Configuration["StoredFilesPath"];
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off });
            }
            return cloudBlobContainer;
        }
    }
}
