namespace WebApi.Entities;

public class Payment : BaseEntity
{
    public DateTime Since { get; set; }
    public DateTime Until { get; set; }
    public Client Client { get; set; } = null!;
    public long ClientId { get; set; }
    public Obligation Obligation { get; set; } = null!;
    public long ObligationId { get; set; }
    public Movement Movement { get; set; } = null!;
    public long MovementId { get; set; }
    public DateTime RegisterAt { get; set; }
}