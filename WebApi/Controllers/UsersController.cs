using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Models.Request;
using WebApi.Models.Response;

namespace WebApi.Controllers;
[Route("api/users"),ApiController]
public class UsersController: Controller
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken token)
    {
        var user = new User
        {
            Password = request.Password,
            FullName = request.FullName,
            UserName = request.UserName
        };
        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
        return Ok("Usuario Creado");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(CreateUserRequest request, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(request.UserName), token);
        if (user is null) return NotFound();
        if (!user.Password.Equals(request.Password))
            return BadRequest();
        return Ok(new UserLoginResponse
        {
            Message = $"Bienvenido {user.FullName}, Al Sistema",
            Success = true,
            UserId = user.Id
        });
    }

    [HttpGet("get-user")]
    public async Task<IActionResult> GetUser(long userId, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id==userId, token);
        if (user is null) return NotFound();
        var userResponse = user.Adapt<UserResponse>();
        return Ok(userResponse);
    }
}