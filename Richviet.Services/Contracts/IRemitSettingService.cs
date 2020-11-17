using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IRemitSettingService
    {
        BussinessUnitRemitSetting GetRemitSettingByCountry(string country);

        List<BussinessUnitRemitSetting> GetRemitSettingList();

        bool AddRemitSetting(BussinessUnitRemitSetting remitSetting);

        bool ModifyRemitSetting(BussinessUnitRemitSetting modifyRemitSetting);

        bool DeleteRemitSetting(long id);
    }
}
