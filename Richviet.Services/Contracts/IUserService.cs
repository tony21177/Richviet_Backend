
using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
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

        UserInfoView GetUserInfoById(long id);

        User GetUserById(long id);

        UserArc GetUserArcById(long userId);

        UserRegisterType GetUserRegisterTypeById(long userId);

        bool ReigsterUser(User user, UserArc userArc,UserRegisterType userRegisterType);

        bool ChangeKycStatusByUserId(KycStatusEnum kycStatus, long userId);

        void UpdatePicFileNameOfUserInfo(UserArc userArc, PictureTypeEnum pictureType, String fileName);

        void UpdatePicFileNameOfDraftRemit(RemitRecord remitRecord, PictureTypeEnum pictureType, String fileName);

        void SystemVerifyArcForRegisterProcess(long userId);

        void SystemVerifyArcForRemitProcess(RemitRecord remitRecord, long userId);
    }
}
