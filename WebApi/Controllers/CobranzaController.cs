using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers;
[Route("api/cobranzas"),ApiController]
public class CobranzaController : Controller
{
    private readonly AppDbContext _context;

    public CobranzaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("import-data")]
    public async Task<IActionResult> ImportData(CancellationToken token)
    {
        var filePath = @"C:\Users\Oscar\Downloads\CLIENTES.xlsx";
        FileInfo file = new FileInfo(filePath);
        var clients = new List<Client>();
        using (ExcelPackage package = new ExcelPackage(file))
        {       
            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            int rowCount = worksheet.Dimension.Rows ;
            int colCount = worksheet.Dimension.Columns;

            var rawText = string.Empty;
            for (int row = 1; row <= rowCount; row++)
            {
                try
                {
                    clients.Add(new Client
                    {
                        Dv = Convert.ToInt32(worksheet.Cells[row +1, 2]?.Value ?? -1),
                        FullName = Convert.ToString(worksheet.Cells[row +1, 3]?.Value ?? ""),
                        SalesStatus = Convert.ToString(worksheet.Cells[row +1, 4]?.Value ?? ""),
                        PurchasingStatus = Convert.ToString(worksheet.Cells[row +1, 5]?.Value ?? ""),
                        Status = Convert.ToString(worksheet.Cells[row +1, 6]?.Value ?? ""),
                        Ruc = Convert.ToString(worksheet.Cells[row +1, 7]?.Value ?? ""),
                        Password = Convert.ToString(worksheet.Cells[row +1, 8]?.Value ?? ""),
                        AccountStatus = Convert.ToString(worksheet.Cells[row +1, 9]?.Value ?? ""),
                        Observations = Convert.ToString(worksheet.Cells[row +1, 10]?.Value ?? ""),
                        ServicePrice = decimal.TryParse(worksheet.Cells[row +1, 12]?.Value.ToString() ?? "0",out var price) ? price: 0,
                        PaymentStatus = Convert.ToString(worksheet.Cells[row +1, 13]?.Value ?? ""),
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
               
            }
        }

        await _context.Clients.AddRangeAsync(clients, token);
        await _context.SaveChangesAsync(token);
        return Ok("Clientes Importados con exito");
    }
}