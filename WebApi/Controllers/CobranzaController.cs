using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using WebApi.Common;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Models.Request;
using WebApi.Models.Response;
using WebApi.Services;

namespace WebApi.Controllers;
[Route("api/cobranzas"),ApiController]
public class CobranzaController : Controller
{
    private readonly AppDbContext _context;
    private readonly CobranzaService _service;

    public CobranzaController(AppDbContext context, CobranzaService service)
    {
        _context = context;
        _service = service;
    }
    

    [HttpPost("create-obligation")]
    public async Task<IActionResult> CreateObligation(CreateObligationRequest request,CancellationToken token)
    {
        var obligation = request.Adapt<Obligation>();
        await _context.Obligations.AddAsync(obligation, token);
        await _context.SaveChangesAsync(token);
        return Ok("Creado con exito ");
    }

    [HttpGet("get-obligations")]
    public async Task<IActionResult> GetObligations(long clientId, CancellationToken token)
    {
        var client = await _context.Clients.Include(x => x.Obligations)
            .FirstOrDefaultAsync(x => x.Id == clientId, token);
        if (client is null) return NotFound();
        var obligationsResponse = new ClientObligationResponse
        {
            ClientId = clientId,
            Obligations = client.Obligations.Select(x => x.Id).ToList(),
            ObligationObjects = client.Obligations.Select(x=> new ObligationResponse
            {
                Id = x.Id,
                Name = x.Name
            }).ToList()
        };
        return Ok(obligationsResponse);
    }
    [HttpPost("update-obligations")]
    public async Task<IActionResult> UpdateObligations([FromBody] UpdateObligationRequest request, CancellationToken token)
    {
        await _service.ClearObligations(request.ClientId, token);
        var result=await _service.AssignObligations(request.ClientId, request.Obligations, token);
        if (result is null)
            return BadRequest();
        return Ok(result);
    }
    [HttpPost("create-payment-type")]
    public async Task<IActionResult> CreatePaymentType([FromBody] CreatePaymentTypeRequest request, CancellationToken token)
    {
        var paymentType = request.Adapt<PaymentType>();
        await _context.PaymentTypes.AddAsync(paymentType, token);
        await _context.SaveChangesAsync(token);
        return Ok("Tipo de pago Creado con exito");
    }
    [HttpGet("get-payments-types")]
    public async Task<IActionResult> GetPaymentsTypes(CancellationToken token)
    {
        var paymentTypes = await _context.
            PaymentTypes
            .Select(x=> new PaymentTypeResponse
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(token);
        return Ok(paymentTypes);
    }
    [HttpPost("check-payment")]
    public async Task<IActionResult> CheckPayment([FromBody] CheckPaymentRequest request, CancellationToken token)
    {
        var payment =
            await _context.Payments.Include(x=> x.Client)
                .Include(x=> x.Obligation)
                .FirstOrDefaultAsync(x => 
                    x.ClientId==request.ClientId && x.ObligationId==request.ObligationId &&
                    x.StartMonth== request.StartMonth && x.EndMonth==request.EndMonth,
                token);
        if (payment is not null)
            return BadRequest($"Ya existe un pago comprendido entre esas fechas para el Cliente {payment.Client.FullName} Obligación {payment.Obligation.Name}");
        return
            Ok();
    }
    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePaymentType([FromBody] CreatePaymentRequest request, CancellationToken token)
    {
        var payment = request.Adapt<Payment>();
        var difference = request.MonthCount;
        payment.Value *= difference;
        payment.Value = payment.Value - payment.Pending;
        var movement = new Movement
        {
            Concept = $"Pago de Honorarios Profesionales",
            Must = (request.Value * difference ) - request.Pending,
            ToHave = request.Pending,
            UserId = request.UserId,
            CreateAt = DateTime.Now,
            Payments = new()
            {
                payment
            }
        };
        await _context.Movements.AddAsync(movement, token);
        await _context.SaveChangesAsync(token);
        var client = await _context.Clients.AsTracking().FirstOrDefaultAsync(x => x.Id == request.ClientId,token);
        var obligation = await _context.Obligations.FirstOrDefaultAsync(x => x.Id == request.ObligationId,token);
        var paymentType= await _context.PaymentTypes.FirstOrDefaultAsync(x => x.Id == request.PaymentTypeId,token);
        if (client is null || obligation is null || paymentType is null) return BadRequest();
        client.PaymentStatus = $"Ultimo Pago en {DateTime.Now.GetMonthName()} ( {obligation.Name} )";
        client.ServicePrice = request.Value;
        await _context.SaveChangesAsync(token);
        _context.ChangeTracker.Clear();
        return Ok(new PaymentResponse
        {
            Message = "Se ha registrado correctamente el pago !",
            Client = client.FullName,
            Ruc = $"{client.Ruc}-{client.Dv}",
            Tot = payment.Value,
            DateTime = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}",
            RowsResponses = new()
            {
                new()
                {
                    Description = ($"PAGO {obligation.Name} " +
                                   (request.StartMonth == request.EndMonth
                                       ? $" mes {Statics.Months[request.StartMonth]}"
                                       : $" mes {Statics.Months[request.StartMonth]} y {Statics.Months[request.EndMonth]}") +
                                   (request.Pending > 0 ? $"(Saldo:{request.Pending:N1})" : "")).ToUpper(),
                    Total = payment.Value
                }
            },
            Obligation = obligation.Name,
            PaymentType = paymentType.Name
        });
    }
    [HttpPost("get-payments-by-client")]
    public async Task<IActionResult> GetPaymentsTypes([FromBody] HistoryPaymentsRequest request, bool pending, CancellationToken token)
    {
        var movements = await _context.Payments.Include(x => x.Client)
            .Include(x => x.Obligation)
            .Include(x => x.PaymentType).Include(x => x.Movement)
            .Where(x => x.ClientId == request.ClientId
                        && (x.Movement.CreateAt.Date >= request.Start.Date &&
                            x.Movement.CreateAt.Date <= request.End.Date)
                        && (!pending || x.Pending > 0)
            ).ToListAsync(token);
        var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == request.ClientId, token);
        if (client is null) return BadRequest();
        var response = new ClientStatusResponse
        {
            Info =
                $"Cliente: {client.FullName}\nRUC: {client.Ruc}-{client.Dv}\nStatus de Pagos: {client.PaymentStatus}\nInforme generado desde {request.Start.ToShortDateString()} hasta {request.End.ToShortDateString()}",
            DetailResponses = movements.Select(x => new ClientStatusDetailResponse
            {
                Id = x.Id,
                Concept = $"PAGO DE {x.Obligation.Name} MES {Statics.Months[x.StartMonth]} " +
                          (x.StartMonth == x.EndMonth ? "" : $"Y {Statics.Months[x.EndMonth]}"),
                Date = x.Movement.CreateAt.ToShortDateString(),
                PaymentType = x.PaymentType.Name, Tot = x.Value, Obligation = x.Obligation.Name, Pending = x.Pending
            }).ToList(),
            Tot = movements.Sum(x => x.Value)
        };
        return Ok(response);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeletePayment(long id, CancellationToken token)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id, token);
        if (payment is null) return NotFound();
        var movement = await _context.Movements.FirstOrDefaultAsync(x => x.Id == payment.MovementId, token);
        _context.Payments.Remove(payment);
        _context.Movements.Remove(movement);
        await _context.SaveChangesAsync(token);
        return Ok("Pago Anulado Con exito");
    }
    [HttpPut("pay-pending")]
    public async Task<IActionResult> PayPending([FromBody] PayPendingRequest request, CancellationToken token)
    {
        var payment = await _context.Payments.AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id, token);
        if (payment is null) return NotFound();
        if (request.Value >= payment.Pending)
        {
            payment.Value += payment.Pending;
            payment.Pending = 0;
        }
        else
        {
            payment.Value += request.Value;
            payment.Pending -= request.Value;
        }
        await _context.SaveChangesAsync(token);
        return Ok(
            $"Pago por Saldo Abonado Correctamente, Saldo pendiente de ahora es {$"{payment.Pending:N1}"}");
    }
    [HttpGet("get-all-obligations")]
    public async Task<IActionResult> GetObligations( CancellationToken token)
    {
        var obligations = await _context.Obligations.Take(3).ToListAsync(token);
        var obligationsResponse = obligations.Adapt<List<ObligationResponse>>();
        return Ok(obligationsResponse);
    }
    [HttpGet("reprint-ticket")]
    public async Task<IActionResult> ReprintTicket(long id, CancellationToken token)
    {
        var payment =await _context.Payments.Include(x => x.Client).Include(x => x.Obligation)
            .Include(x => x.PaymentType)
            .Include(x=> x.Movement)
            .FirstOrDefaultAsync(x => x.Id == id, token);
        if (payment is null) return NotFound();
        return Ok(new PaymentResponse
        {
            Message = "Se ha registrado correctamente el pago !",
            Client = payment.Client.FullName,
            Ruc = $"{payment.Client.Ruc}-{payment.Client.Dv}",
            Tot = payment.Value,
            DateTime =
                $"{payment.Movement.CreateAt.ToShortDateString()} {payment.Movement.CreateAt.ToShortTimeString()}",
            RowsResponses = new()
            {
                new()
                {
                    Description =
                        ($"PAGO {payment.Obligation.Name} " +
                         (payment.StartMonth == payment.EndMonth
                             ? $" mes {Statics.Months[payment.StartMonth]}"
                             : $" mes {Statics.Months[payment.StartMonth]} y {Statics.Months[payment.EndMonth]}") +
                         (payment.Pending > 0 ? $"(Saldo:{payment.Pending:N1})" : "")).ToUpper(),
                    Total = payment.Value
                }
            },
            Obligation = payment.Obligation.Name,
            PaymentType = payment.PaymentType.Name
        });
    }
}