using System.Text.Json.Serialization;
using Vb.Base.Schema;

namespace Vb.Schema;

public class EftTransactionRequest : BaseRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    
    public int AccountId { get; set; }
    
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    
    public string SenderAccount { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
}

public class EftTransactionResponse : BaseResponse
{
    [JsonIgnore]
    public int Id { get; set; }
    
    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    
    public string SenderAccount { get; set; }
    public string SenderIban { get; set; }
    public string SenderName { get; set; }
}