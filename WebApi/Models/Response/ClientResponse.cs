namespace WebApi.Models.Response;

public class ClientResponse
{
    public long Id { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Ruc { get; set; }= string.Empty;
    public int Dv { get; set; }
    public decimal ServicePrice { get; set; }
    public string? PaymentStatus { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string FantasyName { get; set; }
    public string Address { get; set; }
    public bool Available { get; set; }
}