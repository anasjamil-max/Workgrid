using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using Workgrid.DTOs.Task;
using Workgrid.Models;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest request)
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

        var task = new WorkTask
        {
            OrganizationId = request.OrganizationId,
            ProjectId = request.ProjectId,
            AssignedToUserId = request.AssignedToUserId,
            CreatedByUserId = userId,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return Ok(task);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetTasks(long projectId)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var project = await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == projectId);

        if (project == null)
        {
            return NotFound("Project not found.");
        }

        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == project.OrganizationId &&
                x.UserId == userId &&
                x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        var tasks = await _context.Tasks
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();

        return Ok(tasks);
    }
}