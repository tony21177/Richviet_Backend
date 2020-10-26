


using Microsoft.EntityFrameworkCore;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Tools.Utility;


namespace Richviet.Services
{
    public class UserService: IUserService
    {
        private readonly IEnumerable<IAuthService> authServices;
        private readonly JwtHandler jwtHandler;
        private readonly GeneralContext dbContext;


        public UserService(IEnumerable<IAuthService> authServices, JwtHandler jwtHandler, GeneralContext dbContext)
        {
            this.authServices = authServices;
            this.jwtHandler = jwtHandler;
            this.dbContext = dbContext;
        }

        public async Task<bool> AddNewUser(UserRegisterType loginUser)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var user = new User();
                await dbContext.User.AddAsync(user);
                dbContext.SaveChanges();
                var userArc = new UserArc()
                {
                    UserId = user.Id
                };
                await dbContext.UserArc.AddAsync(userArc);
                dbContext.SaveChanges();

                
                var userRegisterType = new UserRegisterType()
                {
                    UserId = user.Id,
                    AuthPlatformId = loginUser.AuthPlatformId,
                    RegisterType = loginUser.RegisterType,
                    Email = loginUser.Email,
                    Name = loginUser.Name
                };
                await dbContext.UserRegisterType.AddAsync(userRegisterType);
                dbContext.SaveChanges();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<UserInfoView> GetUser(UserRegisterType loginUser)
        {

            var list = await dbContext.UserInfoView.Where(user => user.AuthPlatformId == loginUser.AuthPlatformId && user.RegisterType == loginUser.RegisterType).ToListAsync();

            var loggedingUser = list.FirstOrDefault();
            return loggedingUser;
        }

        public UserInfoView GetUserById(int id)
        {
            return dbContext.UserInfoView.Where(userInfo => userInfo.Id == id).FirstOrDefault();
        }

        public async Task<bool> VerifyUserInfo(string accessToken,UserRegisterType loginUser)
        {

            switch ((LoginType)loginUser.RegisterType)
            {
                case LoginType.FB:
                    IAuthService authService = authServices.Single(service => service.LoginType == LoginType.FB);
                    return await authService.VerifyUserInfo(accessToken,loginUser);
                default:
                    return false;
            }
        }

        
    }
}
