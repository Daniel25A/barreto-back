namespace WebApi.Models.Request;

public class CreatePaymentRequest
{
    public int StartMonth { get; set; }
    public int EndMonth { get; set; }
    public long ClientId { get; set; }
    public long ObligationId { get; set; }
    public long? MovementId { get; set; }
    public DateTime RegisterAt { get; set; }
    public long PaymentTypeId { get; set; }
    public long UserId { get; set; }
    public decimal Value { get; set; }
    public decimal Pending { get; set; }
    public string DocNumber { get; set; }
    public string Observation { get; set; } = string.Empty;
    public int MonthCount { get; set; }
}