using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services.Services
{
    public class UserAdminService : IUserAdminService
    {
        private readonly GeneralContext dbContext;
        private readonly ILogger logger;

        public UserAdminService(GeneralContext dbContext, ILogger<UserService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public List<UserAdminListDTO> GetUserList()
        {
            var resultsList = from u in dbContext.User
                         join a in dbContext.UserArc on u.Id equals a.UserId
                         join r in dbContext.UserRegisterType on u.Id equals r.UserId
                         select new { u.Id, a.ArcName, a.ArcNo, a.KycStatus, u.Level, r.RegisterTime };
            List<UserAdminListDTO> userList = new List<UserAdminListDTO>();
            foreach(var res in resultsList)
            {
                UserAdminListDTO dto = new UserAdminListDTO
                { 
                    Id = res.Id,
                    Name = res.ArcName,
                    ArcNo = res.ArcNo,
                    KycStatus = res.KycStatus,
                    Level = res.Level,
                    RegisterTime = res.RegisterTime
                };
                userList.Add(dto);
            }
            return userList;
        }

        public List<UserAdminListDTO> GetUserFilterList()
        {
            throw new NotImplementedException();
        }
    }
}
