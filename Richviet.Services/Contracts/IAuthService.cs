using Frontend.DB.EF.Models;
using Richviet.Services.Constants;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IAuthService
    {
        LoginType LoginType { get; }
        Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser);
    }
}
