using System;
using System.Collections.Generic;
using System.Text;
using Frontend.DB.EF.Models;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Services.Users.Command.Adapter.Repositories;
using Richviet.Services.Users.Query;

namespace Richviet.Services.Users.Command.UseCase
{
    public class UserModifier
    {
        private readonly IUserCommandRepository commandRepository;
        private readonly IUserQueryRepositories queryRepositories;

        public UserModifier(IUserCommandRepository commandRepository, IUserQueryRepositories queryRepositories)
        {
            this.commandRepository = commandRepository;
            this.queryRepositories = queryRepositories;
        }

        public void Modify(UserModifyRequest modifyRequest)
        {
           var user =  MapToUser(modifyRequest);
           commandRepository.Modify(user);
        }

        private User MapToUser(UserModifyRequest modifyRequest)
        {
            var user = queryRepositories.FindById(modifyRequest.Id);

            user.Id = modifyRequest.Id;
            user.Phone = modifyRequest.Phone;
            user.Email = modifyRequest.Email;
            user.Gender = modifyRequest.Gender;
            user.Level = modifyRequest.Level;
            user.Birthday = modifyRequest.Birthday;
            user.UserArc.ArcNo = modifyRequest.ArcNo;
            user.UserArc.ArcIssueDate = modifyRequest.ArcIssueDate;
            user.UserArc.ArcName = modifyRequest.ArcName;
            user.UserArc.BackSequence = modifyRequest.BackSequence;
            user.UserArc.Country = modifyRequest.Country;
            user.UserArc.KycStatus = modifyRequest.KycStatus;
            user.UserArc.PassportId = modifyRequest.PassportId;

            return user;
        }
    }
}
