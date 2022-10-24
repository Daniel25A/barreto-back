namespace WebApi.Models.Request;

public class HistoryPaymentsRequest
{
    public long ClientId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}