using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface ICurrencyService
    {
        List<CurrencyCode> GetCureencyByCountry(string country);
    }
}
