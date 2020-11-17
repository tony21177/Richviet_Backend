using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface IArcScanRecordService
    {
       void AddScanRecordForRegiterProcess(ArcScanRecord reocrd, UserArc userArc);

       void AddScanRecordForRemitProcess(ArcScanRecord reocrd, UserArc userArc,RemitRecord remitRecord);
    }
}
