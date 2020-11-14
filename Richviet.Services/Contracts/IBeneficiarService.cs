using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IBeneficiarService
    {
        OftenBeneficiar AddBeneficiar(OftenBeneficiar beneficiar);

        OftenBeneficiar GetBeneficiarById(int id);

        List<OftenBeneficiar> GetAllBeneficiars(int userId);

        OftenBeneficiar ModifyBeneficiar(OftenBeneficiar modifyBeneficiar, OftenBeneficiar originalBeneficiar);

        void DeleteBeneficiar(OftenBeneficiar deleteBeneficiar);

    }
}
