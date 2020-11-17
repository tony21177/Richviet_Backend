using Microsoft.Extensions.Logging;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Linq;

namespace Richviet.Services
{
    public class PayeeTypeService : IPayeeTypeService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public PayeeTypeService(ILogger<PayeeTypeService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public PayeeType GetPayeeTypeByType(PayeeTypeEnum type)
        {
            return dbContext.PayeeType.First<PayeeType>(payeeType => payeeType.Type == (int)type);
        }

        public PayeeType GetPayeeTypeById(long id)
        {
            return dbContext.PayeeType.Find(id);
        }

    }
}
