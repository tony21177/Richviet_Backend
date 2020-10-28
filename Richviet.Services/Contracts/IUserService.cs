﻿
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IUserService
    {
        Task<bool> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser);

        Task<bool> AddNewUser(UserRegisterType loginUser);

        Task<UserInfoView> GetUser(UserRegisterType loginUser);

        UserInfoView GetUserById(int id);
    }
}
