namespace WebApi.Models.Response;

public class PaymentResponse
{
    public string Message { get; set; }
    public List<TicketRowsResponse> RowsResponses { get; set; } = new();
    public string Client { get; set; }
    public string Ruc { get; set; }
    public string DateTime { get; set; }
    public decimal Tot { get; set; }
    public string PaymentType { get; set; }
    public string Obligation { get; set; }
}

public class TicketRowsResponse
{
    public string Description { get; set; } = string.Empty;
    public decimal  Total { get; set; }
}