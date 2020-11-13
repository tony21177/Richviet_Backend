using Richviet.Services.Models;

namespace Richviet.Services.Users.Command.Adapter.Repositories
{
    public interface IUserCommandRepository
    {
        void Modify(User user);
    }
}
