using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class CurrencyInfoDTO
    {
        [SwaggerSchema("id(主鍵pk)")]
        public long id { set; get; }
        [SwaggerSchema("收款國家可使用的幣別")]
        public string currencyName { set; get; }
        [SwaggerSchema("收款國家")]
        public string country { set; get; }


        [SwaggerSchema("需搭配feeType,<br>e.g. feeType=0,fee=150,手續費即150台幣<br/>feeType=1,fee=90,手續費為匯款金額的90%")]
        public double fee { set; get; }

        [SwaggerSchema("0:數量,1:百分比")]
        public int feeType { set; get; }

    }
}
