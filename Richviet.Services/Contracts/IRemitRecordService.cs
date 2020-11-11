using Richviet.Services.Constants;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IRemitRecordService
    {
        RemitRecord GetOngoingRemitRecordByUserId(int userId);

        RemitRecord GetRemitRecordById(int id);

        RemitRecord CreateRemitRecordByUserId(int userId, PayeeTypeEnum payeeTypeEnum);

        public RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord);
    }
}
