using Richviet.Services.Constants;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Richviet.Admin.API.DataContracts.Requests;

namespace Richviet.Services.Contracts
{
    public interface IRemitRecordService
    {
        List<RemitRecord> GetOngoingRemitRecordsByUserArc(UserArc userArc);

        RemitRecord GetDraftRemitRecordByUserArc(UserArc userArc);

        RemitRecord GetRemitRecordById(long id);

        void DeleteRmitRecord(RemitRecord record);

        List<RemitRecord> GetRemitRecordsByUserId(long userId);

        List<RemitRecord> GetAllRemitRecords();

        RemitRecord CreateRemitRecordByUserArc(UserArc userArc, RemitRecord remitRecord, PayeeTypeEnum payeeTypeEnum);

        RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord, DateTime? applyTime);

        void UpdatePicFileNameOfDraftRemit(RemitRecord remitRecord, PictureTypeEnum pictureType, String fileName);
        List<string> GeneratePaymentCode(RemitRecord modifiedRemitRecord);

        Task SystemVerifyArcForRemitProcess(RemitRecord remitRecord, long userId);

        List<RemitRecord> GetRemitFilterRecords(RemitFilterListRequest request);
    }
}
