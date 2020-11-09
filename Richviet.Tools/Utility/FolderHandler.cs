using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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

    }
}
