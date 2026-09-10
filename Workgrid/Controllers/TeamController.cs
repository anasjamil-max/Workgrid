using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using Workgrid.DTOs.Team;
using Workgrid.Models;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/Contoller")]
[Authorize]

public class TeamController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TeamController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]

    public async Task<IActionResult> Create(CreateTeamRequest request)
    {


        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
        var membership = await _context.OrganizationMembers.FirstOrDefaultAsync(x =>
        x.OrganizationId == request.OrganizationId &&
        x.UserId == userId &&
        x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        var team = new Team
        {
            OrganizationId = request.OrganizationId,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
            
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return Ok(team);
    }



    [HttpGet("{organizationId}")]
    public async Task<IActionResult> GetTeams(long organizatioinId)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

        var membership = await _context.OrganizationMembers.FirstOrDefaultAsync(
            x.OrganizationId == organizatioinId &&
            x.UserId == userId &&
            x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        var teams = await _context.Teams
            .Where(x => x.OrganizationId == organizatioinId)
            .ToListAsync();
            return Ok(teams);

        

    
    }



}





