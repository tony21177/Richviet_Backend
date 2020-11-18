using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IRemitRecordService
    {
        RemitRecord GetOngoingRemitRecordByUserArc(UserArc userArc);

        RemitRecord GetRemitRecordById(long id);

        List<RemitRecord> GetRemitRecordsByUserId(long userId);

        RemitRecord CreateRemitRecordByUserArc(UserArc userArc, PayeeTypeEnum payeeTypeEnum);

        public RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord);
    }
}
