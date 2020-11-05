
using Richviet.API.DataContracts.Requests;
using Richviet.Services.Constants;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IUserService
    {
        Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser);

        Task<bool> AddNewUserInfo(UserRegisterType loginUser);

        Task<UserInfoView> GetUserInfo(UserRegisterType loginUser);

        UserInfoView GetUserInfoById(int id);

        User GetUserById(int id);

        UserArc GetUserArcById(int userId);

        bool ReigsterUserById(int id, RegisterRequest registerReq);

        bool ChangeKycStatusByUserId(KycStatusEnum kycStatus, int userId);
    }
}
