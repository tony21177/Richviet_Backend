using Microsoft.EntityFrameworkCore;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Tools.Utility;
using Richviet.API.DataContracts.Requests;

namespace Richviet.Services
{
    public class UserService: IUserService
    {
        private readonly IEnumerable<IAuthService> authServices;
        private readonly GeneralContext dbContext;


        public UserService(IEnumerable<IAuthService> authServices, GeneralContext dbContext)
        {
            this.authServices = authServices;
            this.dbContext = dbContext;
        }

        public async Task<bool> AddNewUserInfo(UserRegisterType loginUser)
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

        public User GetUserById(int id)
        {
            return dbContext.User.Where(user => user.Id == id).FirstOrDefault();
        }

        public UserArc GetUserArcById(int userId)
        {
            return dbContext.UserArc.Where(userArc => userArc.UserId == userId).FirstOrDefault();
        }

        public async Task<UserInfoView> GetUserInfo(UserRegisterType loginUser)
        {

            var list = await dbContext.UserInfoView.Where(user => user.AuthPlatformId == loginUser.AuthPlatformId && user.RegisterType == loginUser.RegisterType).ToListAsync();

            var loggedingUser = list.FirstOrDefault();
            return loggedingUser;
        }

        public UserInfoView GetUserInfoById(int id)
        {
            return dbContext.UserInfoView.Where(userInfo => userInfo.Id == id).FirstOrDefault();
        }

        public async Task<bool> ReigsterUserById(int id, RegisterRequest registerReq)
        {
            User user = dbContext.User.Where(user => user.Id == id).FirstOrDefault();
            UserArc userArc = dbContext.UserArc.Where(userArc => userArc.UserId == id).FirstOrDefault();
            UserInfoView userInfo = dbContext.UserInfoView.Where(userInfo => userInfo.Id == id).FirstOrDefault();

            if (user == null || userArc == null || userInfo == null)
            {
                return false;
            }

            //update user data
            user.Phone = registerReq.phone;
            user.Email = userInfo.LoginPlatformEmal;
            user.Gender = (byte)registerReq.gender;
            user.Birthday = registerReq.birthday;

            //update userArc data
            userArc.ArcName = registerReq.name;
            userArc.Country = registerReq.country;
            userArc.ArcNo = registerReq.personalID;
            userArc.PassportId = registerReq.passportNumber;
            userArc.BackSequence = registerReq.backCode;
            userArc.ArcIssueDate = registerReq.issue;
            userArc.IdImageA = registerReq.certificateA;
            userArc.IdImageB = registerReq.certificateB;

            dbContext.SaveChanges();

            return true;
        }

        public async Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser)
        {

            switch ((LoginType)loginUser.RegisterType)
            {
                case LoginType.FB:
                    IAuthService authService = authServices.Single(service => service.LoginType == LoginType.FB);
                    return await authService.VerifyUserInfo(accessToken, permissions, loginUser);
                default:
                    return false;
            }
        }
    }
}
