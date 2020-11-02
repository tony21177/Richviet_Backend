
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IPayeeRelationService
    {
        List<PayeeRelationType> GetPayeeRelations();

    }
}
