using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace WebApi.Entities;

public class Movement : BaseEntity
{
    public string Concept { get; set; } = string.Empty;
    public User User { get; set; } = null!;
    public long UserId { get; set; }
    public decimal Must { get; set; }
    public decimal ToHave { get; set; }
    public DateTime CreateAt { get; set; }
    public List<Payment> Payments { get; set; }
}