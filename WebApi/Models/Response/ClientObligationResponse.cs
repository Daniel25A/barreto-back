namespace WebApi.Models.Response;

public class ClientObligationResponse
{
    public long ClientId { get; set; }
    public List<long> Obligations { get; set; } = new();
    public List<ObligationResponse> ObligationObjects { get; set; } = new();
}