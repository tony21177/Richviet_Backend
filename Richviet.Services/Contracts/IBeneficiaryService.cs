using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IBeneficiaryService
    {
        OftenBeneficiary AddBeneficiar(OftenBeneficiary Beneficiary);

        OftenBeneficiary GetBeneficiarById(long id);

        List<OftenBeneficiary> GetAllBeneficiars(long userId);

        OftenBeneficiary ModifyBeneficiar(OftenBeneficiary modifyBeneficiar, OftenBeneficiary originalBeneficiar);

        void DeleteBeneficiar(OftenBeneficiary deleteBeneficiar);

    }
}
