using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Richviet.Services.Users.Command.Adapter.Repositories
{
    public class UserCommandDbCommandRepository : IUserCommandRepository
    {
        private readonly GeneralContext _context;

        public UserCommandDbCommandRepository(GeneralContext context)
        {
            _context = context;
        }

        public void Modify(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
