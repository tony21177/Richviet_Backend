using AutoMapper;
using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly IMapper mapper;

        public DiscountService(ILogger<DiscountService> logger, GeneralContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Discount GetDoscountById(int id)
        {
            return dbContext.Discount.Find(id);
        }
    }
}
