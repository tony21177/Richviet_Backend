using AutoMapper;
using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services
{
    public class BeneficiarService : IBeneficiarService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly IMapper mapper;

        public BeneficiarService(ILogger<BeneficiarService> logger, GeneralContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public OftenBeneficiar AddBeneficiar(OftenBeneficiar beneficiar)
        {
            dbContext.OftenBeneficiar.Add(beneficiar);
            dbContext.SaveChanges();
            dbContext.Entry(beneficiar).Reference(beneficiar => beneficiar.PayeeType).Load();
            return beneficiar;
        }

        public void DeleteBeneficiar(OftenBeneficiar deleteBeneficiar)
        {

            var temp = dbContext.OftenBeneficiar.Remove(deleteBeneficiar);
            dbContext.SaveChanges();
            return;
        }

        public List<OftenBeneficiar> GetAllBeneficiars(long userId)
        {
            List<OftenBeneficiar> beneficiars = dbContext.OftenBeneficiar.Where(beneficiar => beneficiar.UserId == userId).ToList<OftenBeneficiar>();
            beneficiars.ForEach(beneficiar => {
                dbContext.Entry(beneficiar).Reference(beneficiar => beneficiar.PayeeRelation).Load();
                dbContext.Entry(beneficiar).Reference(beneficiar => beneficiar.PayeeType).Load();
            });
            return beneficiars;
        }

        public OftenBeneficiar GetBeneficiarById(long id)
        {
            return dbContext.OftenBeneficiar.Find(id);
        }

        public OftenBeneficiar ModifyBeneficiar(OftenBeneficiar modifyBeneficiar,OftenBeneficiar originalBeneficiar)
        {
            
            dbContext.Entry(originalBeneficiar).CurrentValues.SetValues(modifyBeneficiar);
            dbContext.Entry(originalBeneficiar).Property(x => x.CreateTime).IsModified = false;
            dbContext.Entry(originalBeneficiar).Property(x => x.UserId).IsModified = false;
            dbContext.OftenBeneficiar.Update(originalBeneficiar);
            dbContext.SaveChanges();
            dbContext.Entry(originalBeneficiar).Reference(beneficiar => beneficiar.PayeeType).Load();
            dbContext.Entry(originalBeneficiar).Reference(beneficiar => beneficiar.PayeeRelation).Load();
            return originalBeneficiar;
        }
    }
}
