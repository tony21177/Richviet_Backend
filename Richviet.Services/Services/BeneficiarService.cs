using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services
{
    public class BeneficiarService : IBeneficiarService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public BeneficiarService(ILogger<BeneficiarService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public OftenBeneficiar AddBeneficiar(OftenBeneficiar beneficiar)
        {
            OftenBeneficiar oftenBeneficiar = dbContext.OftenBeneficiar.Add(beneficiar).Entity;
            dbContext.SaveChanges();
            return oftenBeneficiar;
        }
    }
}
