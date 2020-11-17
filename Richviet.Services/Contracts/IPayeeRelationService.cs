using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IPayeeRelationService
    {
        List<PayeeRelationType> GetPayeeRelations();

        bool AddPayeeRelation(PayeeRelationType Relation);

        bool ModifyPayeeRelation(PayeeRelationType modifyRelation);

        bool DeletePayeeRelation(long id);
    }
}
