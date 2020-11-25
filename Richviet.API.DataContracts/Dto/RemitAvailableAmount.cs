using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    public class RemitAvailableAmount
    {
        [SwaggerSchema("當月剩餘可匯款額度")]
        public int MonthlyAvailableRemitAmount { get; set; }
        [SwaggerSchema("當年剩餘可匯款額度")]
        public int YearlyAvailableRemitAmount { get; set; }
    }
}
