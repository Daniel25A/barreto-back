using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Models.Request;
using WebApi.Models.Response;

namespace WebApi.Controllers;
[Route("api/clients"),ApiController]
public class ClientController : Controller
{
  private readonly AppDbContext _context;

  public ClientController(AppDbContext context)
  {
    _context = context;
  }

  [HttpGet("get-clients")]
  public async Task<IActionResult> GetClients( CancellationToken token,string searchTerms="search",string end="-1",long obligationId=-1)
  {
    var clients = _context.Clients.Include(x=> x.Obligations).AsQueryable();
    if (!searchTerms.Equals("search"))
      clients =  clients.Where(x => x.FullName!.ToUpper().Contains(searchTerms) || x.Ruc!.Contains(searchTerms)
        || x.FantasyName.Contains(searchTerms)
        )
        .AsQueryable();
    if (!end.Equals("-1"))
      clients = clients.Where(x => x.Ruc!.Substring(x.Ruc.Length-1,x.Ruc.Length).Equals(end))
        .AsQueryable();
    if (obligationId != -1)
      clients = clients.Where(x => x.Obligations.Any(a => a.Id == obligationId))
        .AsQueryable();
    var clientsResponse = (await clients.ToListAsync(token)).Adapt<List<ClientResponse>>();
    return Ok(clientsResponse);
  }
  [HttpGet("get-client")]
  public async Task<IActionResult> GetClient(long clientId, CancellationToken token)
  {
    var client =  await _context.Clients.FirstOrDefaultAsync(x=> x.Id==clientId, cancellationToken: token);
    if (client is null) return NotFound(new ClientResponse());
    var clientResponse = client.Adapt<ClientResponse>();
    return Ok(clientResponse);
  }

  [HttpPost("register-client")]
  public async Task<IActionResult> RegisterClient([FromBody] AddClientRequest request, CancellationToken token)
  {
    var client = request.Adapt<Client>();
    await _context.Clients.AddAsync(client, token);
    await _context.SaveChangesAsync(token);
    return Ok("Cliente Registrado con exito");
  }
  [HttpPut("{clientId:long}/update-client")]
  public async Task<IActionResult> UpdateClient( long clientId, [FromBody] AddClientRequest request, CancellationToken token)
  {
    var client = await _context.Clients.AsTracking().FirstOrDefaultAsync(x => x.Id == clientId, token);
    if (client is null) return NotFound();
    client.FullName = request.FullName;
    client.FantasyName = request.FantasyName;
    client.Address = request.Address;
    client.Ruc = request.Ruc;
    client.ServicePrice = request.ServicePrice;
    client.Available = request.Available;
    client.PhoneNumber = request.PhoneNumber;
    client.Dv = request.Dv;
    await _context.SaveChangesAsync(token);
    return Ok("Cliente Actualizado con exito");
  }
}