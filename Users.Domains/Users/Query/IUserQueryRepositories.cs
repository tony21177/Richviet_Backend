using Frontend.DB.EF.Models;

namespace Users.Domains.Users.Query
{
    public interface IUserQueryRepositories
    {
        User FindById(long id);
    }
}
