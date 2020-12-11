using AutoMapper;
using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Richviet.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly IMapper mapper;

        public BeneficiaryService(ILogger<BeneficiaryService> logger, GeneralContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public OftenBeneficiary AddBeneficiar(OftenBeneficiary Beneficiary)
        {
            dbContext.OftenBeneficiary.Add(Beneficiary);
            dbContext.SaveChanges();
            dbContext.Entry(Beneficiary).Reference(Beneficiary => Beneficiary.PayeeType).Load();
            return Beneficiary;
        }

        public void DeleteBeneficiar(OftenBeneficiary deleteBeneficiar)
        {

            var temp = dbContext.OftenBeneficiary.Remove(deleteBeneficiar);
            dbContext.SaveChanges();
            return;
        }

        public List<OftenBeneficiary> GetAllBeneficiars(long userId)
        {
            return dbContext.OftenBeneficiary.Include(beneficiary=> beneficiary.PayeeRelation).Include(beneficiary => beneficiary.PayeeType).Where(Beneficiary => Beneficiary.UserId == userId).ToList<OftenBeneficiary>();

            //List<OftenBeneficiary> beneficiars = dbContext.OftenBeneficiary.Where(Beneficiary => Beneficiary.UserId == userId).ToList<OftenBeneficiary>();
            //beneficiars.ForEach(Beneficiary => {
            //    dbContext.Entry(Beneficiary).Reference(Beneficiary => Beneficiary.PayeeRelation).Load();
            //    dbContext.Entry(Beneficiary).Reference(Beneficiary => Beneficiary.PayeeType).Load();
            //});
        }

        public OftenBeneficiary GetBeneficiarById(long id)
        {
            return dbContext.OftenBeneficiary.Find(id);
        }

        public OftenBeneficiary ModifyBeneficiar(OftenBeneficiary modifyBeneficiar,OftenBeneficiary originalBeneficiar)
        {
            
            dbContext.Entry(originalBeneficiar).CurrentValues.SetValues(modifyBeneficiar);
            dbContext.Entry(originalBeneficiar).Property(x => x.CreateTime).IsModified = false;
            dbContext.Entry(originalBeneficiar).Property(x => x.UserId).IsModified = false;
            dbContext.OftenBeneficiary.Update(originalBeneficiar);
            dbContext.SaveChanges();
            dbContext.Entry(originalBeneficiar).Reference(Beneficiary => Beneficiary.PayeeType).Load();
            dbContext.Entry(originalBeneficiar).Reference(Beneficiary => Beneficiary.PayeeRelation).Load();
            return originalBeneficiar;
        }
    }
}
