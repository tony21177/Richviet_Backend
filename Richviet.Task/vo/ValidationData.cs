using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Richviet.Task.vo
{
    class ValidationData
    { 
        [JsonPropertyName("IDNO")]
        public string Idno { set; get; }
        [JsonPropertyName("APPROVE_DATE")]
        public string ApproveDate { set; get; }
        [JsonPropertyName("BARCODE_NO")]
        public string BarcodeNo { set; get; }
        [JsonPropertyName("TextBox1")]
        public string TextBox1 { set; get; }
        [JsonPropertyName("ReNext")]
        public string ReNext { set; get; }
        [JsonPropertyName("__VIEWSTATE")]
        public string ViewState { set; get; }
        [JsonPropertyName("__VIEWSTATEGENERATOR")]
        public string ViewStateGenerator { set; get; }
        [JsonPropertyName("__EVENTVALIDATION")]
        public string EventValidation { set; get; }

        public string SessionId { set; get; }

    }
}
