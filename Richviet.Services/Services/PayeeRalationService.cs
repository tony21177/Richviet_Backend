using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Richviet.Services
{
    public class PayeeRalationService : IPayeeRelationService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public PayeeRalationService(ILogger<PayeeRalationService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public List<PayeeRelationType> GetPayeeRelations()
        {
            return dbContext.PayeeRelationType.ToList();
        }

        public bool AddPayeeRelation(PayeeRelationType Relation)
        {
            try
            {
                dbContext.PayeeRelationType.Add(Relation);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public bool DeletePayeeRelation(int id)
        {
            try
            {
                PayeeRelationType relation = dbContext.PayeeRelationType.Single(x => x.Id == id);
                if (relation != null)
                {
                    dbContext.PayeeRelationType.Remove(relation);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public bool ModifyPayeeRelation(PayeeRelationType modifyRelation)
        {
            try
            {
                PayeeRelationType relation = dbContext.PayeeRelationType.Single(x => x.Id == modifyRelation.Id);
                dbContext.Entry(relation).CurrentValues.SetValues(modifyRelation);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }
    }
}
