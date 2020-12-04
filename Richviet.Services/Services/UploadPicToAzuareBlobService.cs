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
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using System.Collections;

namespace Richviet.Services.Services
{
    public class UploadPicToAzuareBlobService : IUploadPic
    {
        public IConfiguration Configuration { get; }
        string connectionString = string.Empty;


        public UploadPicToAzuareBlobService(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("AccessKey");

        }

        public async Task<List<String>> GetBlobList(String containerName)
        {
            BlobContainerClient blobContainer = await GetOrCreateCloudBlobContainer(containerName, PublicAccessType.Blob);
            Pageable<BlobItem> blobItems = blobContainer.GetBlobs();
            var containerUri = blobContainer.Uri.AbsoluteUri;
            List<String> result = new List<string>();
            
            foreach (BlobItem blobItem in blobItems){
                var name = Uri.EscapeDataString(blobItem.Name);
                result.Add(containerUri +"/"+ name);
            }
            return result;
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
                BlobContainerClient blobContainer = await GetOrCreateCloudBlobContainer(GetUploadPicContainerName(), PublicAccessType.None);

                if (fileFullName != null && image != null)
                {
                   
                    BlobClient blobClient = blobContainer.GetBlobClient(fileFullName);
                    await blobClient.UploadAsync(image.OpenReadStream());
                    
                    return blobClient.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Stream> LoadImage(UserArc userArc, byte imageType,string imageFileUri)
        {
            String [] imageUriArray = imageFileUri.Split('/');
            String imageFileName = WebUtility.UrlDecode(imageUriArray[imageUriArray.Length - 1]);
            BlobContainerClient blobContainer = await GetOrCreateCloudBlobContainer(GetUploadPicContainerName(), PublicAccessType.None);
            BlobClient imageBlob = blobContainer.GetBlobClient(imageFileName);
            if (!imageBlob.Exists()) return null;
            return await imageBlob.OpenReadAsync();
        }

        public async Task<bool> CheckUploadFileExistence(UserArc userArc, PictureTypeEnum typeEnum, String imageFileUri)
        {
            String[] imageUriArray = imageFileUri.Split('/');
            String imageFileName = WebUtility.UrlDecode(imageUriArray[imageUriArray.Length - 1]);
            BlobContainerClient blobContainer = await GetOrCreateCloudBlobContainer(GetUploadPicContainerName(),PublicAccessType.None);
            BlobClient imageBlob = blobContainer.GetBlobClient(imageFileName);
            bool isExist = await imageBlob.ExistsAsync();
            return isExist;
        }

        private async Task<BlobContainerClient> GetOrCreateCloudBlobContainer(String strContainerName, PublicAccessType type)
        {
  
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(strContainerName);
            await containerClient.CreateIfNotExistsAsync(publicAccessType: type);
            return containerClient;
        }

        private string GetUploadPicContainerName()
        {
            return Configuration["StoredFilesPath"];
        }

        
    }
}
