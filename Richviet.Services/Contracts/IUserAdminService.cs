using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Admin.API.DataContracts.Requests;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IUserAdminService
    {
        List<UserAdminListDTO> GetUserList();

        List<UserAdminListDTO> GetUserFilterList(UserFilterListRequest request);

        UserDetailDTO GetUserDetail(long userId);
    }
}
