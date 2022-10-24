namespace WebApi.Models.Response;

public class ClientStatusResponse
{
    public string Info { get; set; }
    public decimal Tot { get; set; }
    public List<ClientStatusDetailResponse> DetailResponses { get; set; }
}

public class ClientStatusDetailResponse
{
    public long Id { get; set; }
    public string Concept { get; set; }
    public string Date { get; set; }
    public string PaymentType { get; set; }
    public decimal Tot { get; set; }
    public string Obligation { get; set; }
    public decimal Pending { get; set; }
}