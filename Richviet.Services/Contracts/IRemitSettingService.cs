using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IRemitSettingService
    {
        BussinessUnitRemitSetting GetRemitSettingByCountry(string country);
    }
}
