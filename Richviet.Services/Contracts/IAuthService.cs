using Richviet.Services.Constants;
using Richviet.Services.Models;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IAuthService
    {
        LoginType LoginType { get; }
        Task<bool> VerifyUserInfo(string accessToken,UserRegisterType loginUser);
    }
}
