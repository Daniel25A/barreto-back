using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Controllers;
[Route("api/movements"),ApiController]
public class MovementsController : Controller
{
    private readonly AppDbContext _context;

    public MovementsController(AppDbContext context)
    {
        _context = context;
    }
}