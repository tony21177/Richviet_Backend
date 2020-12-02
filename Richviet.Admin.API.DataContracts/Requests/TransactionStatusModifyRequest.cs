using Swashbuckle.AspNetCore.Annotations;

namespace Richviet.Admin.API.DataContracts.Requests
{
    public class TransactionStatusModifyRequest
    {
        [SwaggerSchema("匯款交易識別碼")]
        public long RecordId { get; set; }

        [SwaggerSchema("備註")]
        public string Comment { get; set; }

    }
}