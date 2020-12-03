using Frontend.DB.EF.Models;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Command.Request;
using Users.Domains.Users.Query;

namespace Users.Domains.Users.Command.UseCase
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

        public void Modify(UserVoRequest modifyRequest)
        {
           var user =  MapToUser(modifyRequest);
           commandRepository.Modify(user);
        }

        public void ModifyUserLevel(UserLevelModifyVoRequest request)
        {
            var user = this.queryRepositories.FindById(request.Id);
            user.Level = request.Level;
            commandRepository.Modify(user);
        }


        private User MapToUser(UserVoRequest modifyRequest)
        {
            var user = queryRepositories.FindById(modifyRequest.Id);

            user.Id = modifyRequest.Id;
            user.Phone = modifyRequest.Phone;
            if(modifyRequest.Email!=null) user.Email = modifyRequest.Email;
            user.Gender = modifyRequest.Gender;
            user.Level = modifyRequest.Level;
            user.Birthday = modifyRequest.Birthday;
            user.UserArc.ArcNo = modifyRequest.ArcNo;
            user.UserArc.ArcIssueDate = modifyRequest.ArcIssueDate;
            user.UserArc.ArcExpireDate = modifyRequest.ArcExpireDate;
            user.UserArc.ArcName = modifyRequest.ArcName;
            user.UserArc.BackSequence = modifyRequest.BackSequence;
            user.UserArc.Country = modifyRequest.Country;
            user.UserArc.KycStatus = (short)modifyRequest.KycStatus;
            user.UserArc.PassportId = modifyRequest.PassportId;

            return user;
        }


    }
}
