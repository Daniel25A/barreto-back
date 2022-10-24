using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Services;

public class CobranzaService
{
    private readonly AppDbContext _context;

    public CobranzaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ClearObligations(long clientId, CancellationToken token)
    {
        var client = await _context.Clients.Include(x => x.Obligations)
            .AsTracking().FirstOrDefaultAsync(x => x.Id == clientId, token);
        if (client is null) return;
        client.Obligations.Clear();
        await _context.SaveChangesAsync(token);
        _context.ChangeTracker.Clear();
    }
    public async Task<string?> AssignObligations(long clientId,List<long> obligations, CancellationToken token)
    {
        var client = await _context.Clients.Include(x => x.Obligations)
            .AsTracking().FirstOrDefaultAsync(x => x.Id == clientId, token);
        if (client is null) return null;
        client.Obligations = obligations.Select(x => new Obligation
        {
            Id = x
        }).ToList();
        _context.AttachRange(client.Obligations);
        await _context.SaveChangesAsync(token);
        _context.ChangeTracker.Clear();
        return $"Obligaciones Actualizadas con exito para el Cliente {client.FullName}";
    }
}