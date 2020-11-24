using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IRemitRecordService
    {
        List<RemitRecord> GetOngoingRemitRecordsByUserArc(UserArc userArc);

        RemitRecord GetDraftRemitRecordByUserArc(UserArc userArc);

        RemitRecord GetRemitRecordById(long id);

        void DeleteRmitRecord(RemitRecord record);

        List<RemitRecord> GetRemitRecordsByUserId(long userId);

        RemitRecord CreateRemitRecordByUserArc(UserArc userArc, PayeeTypeEnum payeeTypeEnum);

        RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord);
    }
}
