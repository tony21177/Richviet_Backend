using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Richviet.Services.Users.Query
{
    public class UserQueryRepositories : IUserQueryRepositories
    {

        private readonly GeneralContext _context;

        public UserQueryRepositories(GeneralContext context)
        {
            _context = context;
        }

        public User FindById(long id)
        {
            var users = _context.User.Include(u => u.UserArc)
                .Where(x => x.Id == id);
            var user = users.SingleOrDefault();
            return user;
        }
    }
}
