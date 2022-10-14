namespace WebApi.Models.Response;

public class ClientResponse
{
    public long Id { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? SalesStatus { get; set; }= string.Empty;
    public string? PurchasingStatus { get; set; }= string.Empty;
    public string? Status { get; set; }= string.Empty;
    public string? Ruc { get; set; }= string.Empty;
    public int Dv { get; set; }
    public string? Password { get; set; }= string.Empty;
    public string? AccountStatus { get; set; }= string.Empty;
    public string? Observations { get; set; }= string.Empty;
    public decimal ServicePrice { get; set; }
    public string? PaymentStatus { get; set; } = string.Empty;
}