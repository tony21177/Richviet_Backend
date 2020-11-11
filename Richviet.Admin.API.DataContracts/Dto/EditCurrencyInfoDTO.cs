using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class EditCurrencyInfoDTO
    {
        [SwaggerSchema("收款國家手續費設定id")]
        public int Id { get; set; }

        [SwaggerSchema("收款國家可使用的幣別")]
        public string CurrencyName { set; get; }

        [SwaggerSchema("收款國家")]
        public string Country { set; get; }

        [SwaggerSchema("需搭配feeType,<br>e.g. feeType=0,fee=150,手續費即150台幣<br/>feeType=1,fee=90,手續費為匯款金額的90%")]
        public double Fee { set; get; }

        [SwaggerSchema("0:數量,1:百分比")]
        public int FeeType { set; get; }
    }
}
