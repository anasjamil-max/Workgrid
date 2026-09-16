
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

        var project = await _context.Projects
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProjectId &&
                x.OrganizationId == request.OrganizationId);

        if (project == null)
        {
            return NotFound("Project not found in this organization.");
        }

        if (request.AssignedToUserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(x => x.Id == request.AssignedToUserId.Value);

            if (!userExists)
            {
                return NotFound("Assigned user not found.");
            }

            var assignedUser = await _context.OrganizationMembers
                .FirstOrDefaultAsync(x =>
                    x.OrganizationId == request.OrganizationId &&
                    x.UserId == request.AssignedToUserId.Value &&
                    x.IsActive);

            if (assignedUser == null)
            {
                return BadRequest(
                    "Assigned user is not a member of this organization.");
            }
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


    


    [HttpPut("{taskId}")]
    public async Task<IActionResult> Update(long taskId, UpdateTaskRequest request)
    {

        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );


        var task = await _context.Tasks
           .FirstOrDefaultAsync(x => x.Id == taskId);


        if (task == null)
        {
            return NotFound("Task not found.");
        }


        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == task.OrganizationId &&
                x.UserId == userId &&
                x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        if (request.AssignedToUserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(x => x.Id == request.AssignedToUserId.Value);

            if (!userExists)
            {
                return NotFound("Assigned user not found.");
            }

            var assignedUser = await _context.OrganizationMembers
                .FirstOrDefaultAsync(x =>
                    x.OrganizationId == task.OrganizationId &&
                    x.UserId == request.AssignedToUserId.Value &&
                    x.IsActive);

            if (assignedUser == null)
            {
                return BadRequest(
                    "Assigned user is not a member of this organization.");

            }
        }

        task.AssignedToUserId = request.AssignedToUserId;
        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.UpdatedAt = DateTime.UtcNow;


        await _context.SaveChangesAsync();

        return Ok(task);

    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> Delete(long taskId)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );


        var task = await _context.Tasks
           .FirstOrDefaultAsync(x => x.Id == taskId);


        if (task == null)
        {
            return NotFound("Task not found.");
        }


        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == task.OrganizationId &&
                x.UserId == userId &&
                x.IsActive);

        if (membership == null)
        {
            return Forbid();
        }

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();
        return Ok("Task deleted successfully.");

    }

    

}
