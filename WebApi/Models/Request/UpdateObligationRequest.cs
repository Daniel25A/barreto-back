namespace WebApi.Models.Request;

public class UpdateObligationRequest
{
    public long ClientId { get; set; }
    public List<long> Obligations { get; set; } = new();
}