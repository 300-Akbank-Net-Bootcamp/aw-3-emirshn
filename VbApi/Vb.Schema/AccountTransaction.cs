using System.Text.Json.Serialization;
using Vb.Base.Schema;

namespace Vb.Schema;

public class AccountTransactionRequest : BaseRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    
    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransferType { get; set; }
}
public class AccountTransactionResponse : BaseResponse
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransferType { get; set; }
}