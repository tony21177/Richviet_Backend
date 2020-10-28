using Richviet.Services.Constants;
using Richviet.Services.Models;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IAuthService
    {
        LoginType LoginType { get; }
        Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser);
    }
}
