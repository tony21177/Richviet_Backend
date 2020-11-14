using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
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

        public bool AddReceiveBank(ReceiveBank bank)
        {
            try
            {
                dbContext.ReceiveBank.Add(bank);
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;          
        }

        public bool DeleteReceiveBank(int id)
        {
            try
            {
                ReceiveBank bank = dbContext.ReceiveBank.Single(x => x.Id == id);
                if (bank != null)
                {
                    dbContext.ReceiveBank.Remove(bank);
                    dbContext.SaveChanges();
                    return true;
                }          
            }
            catch(Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }     

        public bool ModifyReceiveBank(ReceiveBank modifyBank)
        {
            try
            {
                ReceiveBank bank = dbContext.ReceiveBank.Single(x => x.Id == modifyBank.Id);
                dbContext.Entry(bank).CurrentValues.SetValues(modifyBank);
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }
    }
}
