using System;
using System.Collections.Generic;
using System.Text;
using Frontend.DB.EF.Models;

namespace Richviet.Services.Users.Query
{
    public interface IUserQueryRepositories
    {
        User FindById(long id);
    }
}
