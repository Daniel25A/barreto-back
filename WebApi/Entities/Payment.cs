namespace WebApi.Entities;

public class Payment : BaseEntity
{
    public int StartMonth { get; set; }
    public int EndMonth { get; set; }
    public int MonthCount { get; set; }
    public Client Client { get; set; } = null!;
    public long ClientId { get; set; }
    public Obligation Obligation { get; set; } = null!;
    public long ObligationId { get; set; }
    public Movement Movement { get; set; } = null!;
    public long? MovementId { get; set; }
    public DateTime RegisterAt { get; set; }
    public PaymentType PaymentType { get; set; } = null!;
    public long PaymentTypeId { get; set; }
    public User User { get; set; } = null!;
    public long UserId { get; set; }
    public decimal Value { get; set; }
    public decimal Pending { get; set; }
    public string DocNumber { get; set; }
    public string Observation { get; set; } = string.Empty;
}