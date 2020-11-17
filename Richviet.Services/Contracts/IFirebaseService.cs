using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IFirebaseService
    {
        void SendPush(string mobileToken, string title, string body);

        bool UpdateMobileToken(int userId, string mobileToken);
    }
}
