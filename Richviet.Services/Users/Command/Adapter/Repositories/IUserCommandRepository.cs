

using Frontend.DB.EF.Models;

namespace Richviet.Services.Users.Command.Adapter.Repositories
{
    public interface IUserCommandRepository
    {
        void Modify(User user);
    }
}
