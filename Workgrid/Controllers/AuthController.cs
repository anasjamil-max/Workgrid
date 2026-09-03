using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using Workgrid.DTOs.Auth;
using Workgrid.Models;
using Workgrid.Services;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordService _passwordService;

    public AuthController(
        ApplicationDbContext context,
        PasswordService passwordService)
    {
        _context = context;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existingUser != null)
        {
            return BadRequest("Email already registered.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordService.HashPassword(
            user,
            request.Password
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registration successful.");
    }
}