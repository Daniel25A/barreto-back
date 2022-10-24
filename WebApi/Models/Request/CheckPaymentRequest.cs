namespace WebApi.Models.Request;

public class CheckPaymentRequest
{
    public long ClientId { get; set; }
    public long ObligationId { get; set; }
    public int StartMonth { get; set; }
    public int EndMonth { get; set; }
}