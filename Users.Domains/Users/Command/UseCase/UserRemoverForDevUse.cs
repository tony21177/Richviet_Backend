using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Query;

namespace Users.Domains.Users.Command.UseCase
{
    public class UserRemoverForDevUse
    {
        private readonly IUserCommandRepository commandRepository;
        private readonly IUserQueryRepositories queryRepositories;

        public UserRemoverForDevUse(IUserCommandRepository commandRepository, IUserQueryRepositories queryRepositories)
        {
            this.commandRepository = commandRepository;
            this.queryRepositories = queryRepositories;
        }

        public int removeUser(long userId)
        {
            return commandRepository.DeleteUser(userId);
        }


    }
}
