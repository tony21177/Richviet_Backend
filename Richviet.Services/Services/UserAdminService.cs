﻿using Microsoft.Extensions.Logging;
using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Richviet.Services.Constants;
using Microsoft.EntityFrameworkCore;
using Richviet.Tools.Utility;

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
            var resList = from u in dbContext.User
                         join a in dbContext.UserArc on u.Id equals a.UserId
                         join r in dbContext.UserRegisterType on u.Id equals r.UserId
                         select new { u.Id, a.ArcName, a.ArcNo, a.KycStatus, u.Level, r.RegisterTime };
            List<UserAdminListDTO> userList = new List<UserAdminListDTO>();
            foreach(var res in resList)
            {
                UserAdminListDTO dto = new UserAdminListDTO
                { 
                    Id = (int)res.Id,
                    Name = res.ArcName,
                    ArcNo = res.ArcNo,
                    KycStatus = (short)res.KycStatus,
                    Level = res.Level,
                    RegisterTime = res.RegisterTime
                };
                userList.Add(dto);
            }
            return userList;
        }

        public List<UserAdminListDTO> GetUserFilterList(UserFilterListRequest request)
        {
            var resList = from u in dbContext.User
                          join a in dbContext.UserArc on u.Id equals a.UserId       
                          join r in dbContext.UserRegisterType on u.Id equals r.UserId
                          select new { u.Id, a.ArcName, a.ArcNo, a.KycStatus, u.Level, r.RegisterTime };
            resList = resList.Where(x => x.ArcName.Contains(request.Name) && x.ArcNo.Contains(request.ArcNo) &&
                                        (TimeUtil.LongSpanToUtcDateTime(request.RegisterStartTime) <= x.RegisterTime) && 
                                        (TimeUtil.LongSpanToUtcDateTime(request.RegisterEndTime) >= x.RegisterTime));
            if(request.KycFormal || request.KycUnderReview || request.KycDraft || request.KycDisabled || request.LevelNormal ||
                request.LevelVIP || request.LevelRisk)
            {
                resList = resList.Where(x => (request.KycFormal && (x.KycStatus == (short)KycStatusEnum.PASSED_KYC_FORMAL_MEMBER)) ||
                                         (request.KycUnderReview && (x.KycStatus == (short)KycStatusEnum.WAITING_VERIFIED_KYC)) ||
                                         (request.KycDraft && (x.KycStatus == (short)KycStatusEnum.DRAFT_MEMBER)) ||
                                         (request.KycDisabled && (x.KycStatus == (short)KycStatusEnum.FAILED_KYC))||
                                         (request.LevelNormal && (x.Level == (short)UserLevelEnum.NORMAL)) ||
                                         (request.LevelVIP && (x.Level == (short)UserLevelEnum.VIP)) ||
                                         (request.LevelRisk && (x.Level == (short)UserLevelEnum.RISK)) );
            }            
            List <UserAdminListDTO> userList = new List<UserAdminListDTO>();
            foreach (var res in resList)
            {
                UserAdminListDTO dto = new UserAdminListDTO
                {
                    Id = (int)res.Id,
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

        public UserDetailDTO GetUserDetail(long userId)
        {
            var res = from u in dbContext.User where u.Id == userId
                      join a in dbContext.UserArc on u.Id equals a.UserId
                      join g in dbContext.UserLoginLog on u.Id equals g.UserId
                      orderby g.LoginTime descending
                      select new { u.Id, Name = a.ArcName, a.ArcNo, a.KycStatus, u.Level, u.Gender, a.Country,
                          u.Birthday, a.PassportId, a.ArcIssueDate, a.ArcExpireDate, a.BackSequence,
                          u.Phone, g.LoginTime, g.Address, a.IdImageA, a.IdImageB };
            UserDetailDTO dto = new UserDetailDTO();
            foreach (var r in res.Take(1))
            {
                dto.Id = (int)r.Id;
                dto.Name = r.Name;
                dto.ArcNo = r.ArcNo;
                dto.KycStatus = r.KycStatus;
                dto.Level = r.Level;
                dto.Gender = (byte)r.Gender;
                dto.Country = r.Country;
                dto.Birthday = r.Birthday;
                dto.PassportId = r.PassportId;
                dto.ArcIssueDate = r.ArcIssueDate;
                dto.ArcExpireDate = r.ArcExpireDate;
                dto.BackSequence = r.BackSequence;
                dto.Phone = r.Phone;
                dto.LoginTime = (long)((DateTime)r.LoginTime).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                dto.Address = r.Address;
                dto.IdImageA = r.IdImageA;
                dto.IdImageB = r.IdImageB;
            }
            return dto;
        }
    }
}
