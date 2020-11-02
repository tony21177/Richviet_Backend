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
    }
}
