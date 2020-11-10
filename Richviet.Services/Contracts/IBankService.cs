
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IBankService
    {
        List<ReceiveBank> GetReceiveBanks();

        bool AddReceiveBank(ReceiveBank bank);

        bool ModifyReceiveBank(ReceiveBank modifyBank);

        bool DeleteReceiveBank(int id);
    }
}
