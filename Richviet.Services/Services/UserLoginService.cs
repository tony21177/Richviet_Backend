using Frontend.DB.EF.Models;
using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Services
{
    public class UserLoginService : IUserLoginLogService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public UserLoginService(ILogger<IUserLoginLogService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public void AddLoginLog(UserLoginLog userLogingLog)
        {
            dbContext.UserLoginLog.Add(userLogingLog);
            dbContext.SaveChanges();
        }
    }
}
