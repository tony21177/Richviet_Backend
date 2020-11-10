using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Richviet.Services
{
    public class BankService : IBankService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public BankService(ILogger<BankService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public List<ReceiveBank> GetReceiveBanks()
        {
            return dbContext.ReceiveBank.ToList();
        }

        public void AddReceiveBank(ReceiveBank bank)
        {
            dbContext.ReceiveBank.Add(bank);
        }

        public void DeleteReceiveBank(ReceiveBank bank)
        {
            
        }     

        public void ModifyReceiveBank(ReceiveBank modifyBank, ReceiveBank oldBank)
        {
            throw new NotImplementedException();
        }
    }
}
