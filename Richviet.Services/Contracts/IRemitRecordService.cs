﻿using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IRemitRecordService
    {
        List<RemitRecord> GetOngoingRemitRecordsByUserArc(UserArc userArc);

        RemitRecord GetDraftRemitRecordByUserArc(UserArc userArc);

        RemitRecord GetRemitRecordById(long id);

        void DeleteRmitRecord(RemitRecord record);

        List<RemitRecord> GetRemitRecordsByUserId(long userId);

        RemitRecord CreateRemitRecordByUserArc(UserArc userArc, RemitRecord remitRecord, PayeeTypeEnum payeeTypeEnum);

        RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord, DateTime? applyTime);
        List<string> GeneratePaymentCode(RemitRecord modifiedRemitRecord);

        Task SystemVerifyArcForRemitProcess(RemitRecord remitRecord, long userId);
    }
}
