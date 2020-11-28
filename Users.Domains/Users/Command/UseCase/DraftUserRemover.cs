using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Query;

namespace Users.Domains.Users.Command.UseCase
{
    public class DraftUserRemover
    {
        private readonly IUserCommandRepository commandRepository;
        private readonly IUserQueryRepositories queryRepositories;

        public DraftUserRemover(IUserCommandRepository commandRepository, IUserQueryRepositories queryRepositories)
        {
            this.commandRepository = commandRepository;
            this.queryRepositories = queryRepositories;
        }

        public int Execute(int days)
        {
            return commandRepository.DeleteDraftUserAfterDays(days);
        }
    }
}
