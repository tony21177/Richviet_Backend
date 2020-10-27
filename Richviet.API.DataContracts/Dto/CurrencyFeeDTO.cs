using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class CurrencyFeeDTO
    {
        public string currencyName { set; get; }


        [SwaggerSchema("需搭配feeType,<br>e.g. feeType=0,fee=150,手續費即150台幣<br/>feeType=1,fee=90,手續費為匯款金額的90%")]
        public double fee { set; get; }

        [SwaggerSchema("0:數量,1:百分比")]
        public int feeType { set; get; }

    }
}
