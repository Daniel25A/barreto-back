namespace WebApi.Entities;

public class Client : BaseEntity
{
    public string? FullName { get; set; } = string.Empty;
    public string? SalesStatus { get; set; }= string.Empty;
    public string? PurchasingStatus { get; set; }= string.Empty;
    public string? Status { get; set; }= string.Empty;
    public string? Ruc { get; set; }= string.Empty;
    public int Dv { get; set; }
    public string? Password { get; set; }= string.Empty;
    public string? AccountStatus { get; set; }= string.Empty;
    public string? Observations { get; set; }= string.Empty;
    public List<Obligation> Obligations { get; set; } = new();
    public decimal ServicePrice { get; set; }
    public string? PaymentStatus { get; set; } = string.Empty;
}