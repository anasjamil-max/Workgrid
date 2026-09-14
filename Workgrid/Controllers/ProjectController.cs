using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using System.Security.Claims;
using Workgrid.DTOs.Project;
using Workgrid.Models;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == request.OrganizationId &&
                x.UserId == userId &&
                x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        var project = new Project
        {
            OrganizationId = request.OrganizationId,
            TeamId = request.TeamId,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            StartDate = request.StartDate,
            DueDate = request.DueDate,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return Ok(project);
    }



    //http get

    [HttpGet("{organizationId}")]

    public async Task<IActionResult> GetProjects(long organizationId)
    {

        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
        var partOf = await _context.OrganizationMembers.FirstOrDefaultAsync(x =>
            x.OrganizationId == organizationId &&
            x.UserId == userId &&
            x.IsActive);

        if (partOf == null)
        {
            return Forbid();

        }

        var projects = await _context.Projects
            .Where(x => x.OrganizationId == organizationId)
            .ToListAsync();

        return Ok(projects);



    }



}