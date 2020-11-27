using System;
using Frontend.DB.EF.Models;

namespace Users.Domains.Users.Command.Adapter.Repositories
{

    public interface IUserCommandRepository
    {
        void Modify(User user);

        int DeleteDraftUser();

        int DeleteDraftUserAfterDays(int days);
    }
}