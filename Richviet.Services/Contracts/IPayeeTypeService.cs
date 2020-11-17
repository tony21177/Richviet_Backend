using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IPayeeTypeService
    {
        PayeeType GetPayeeTypeByType(PayeeTypeEnum payeeType);

        PayeeType GetPayeeTypeById(long id);
    }
}
