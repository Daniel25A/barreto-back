using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
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
  public async Task<IActionResult> GetClients( CancellationToken token,string searchTerms="search")
  {
    var clients = _context.Clients.AsQueryable();
    if (!searchTerms.Equals("search"))
      clients =  clients.Where(x => x.FullName!.ToUpper().Contains(searchTerms) || x.Ruc!.Contains(searchTerms))
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
}