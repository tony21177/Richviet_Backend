using System;
using System.Collections.Generic;
using System.Text;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Services.Users.Command.Adapter.Repositories;

namespace Richviet.Services.Users.Command.UseCase
{
    public class UserModifier
    {
        private IUserCommandRepository commandRepository;

        public UserModifier(IUserCommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public void Modify(UserModifyRequest modifyRequest)
        {
         
        }
    }
}
