using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workgrid.DTOs.Organization;
using Workgrid.Models;
using Workgrid.Data;

namespace Workgrid.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class OrganizationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public OrganizationController(ApplicationDbContext context)
    {

        _context = context;

    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrganizationRequest request)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );


        var organization = new Organization
        {

            Name = request.Name,
            Description = request.Description,
            Slug = request.Name.ToLower().Replace(" ", "-"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };


        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();

        var membership = new OrganizationMember
        { 
            OrganizationId = organization.Id,
            UserId = userId,
            Role = "Owner",
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };


        _context.OrganizationMembers.Add(membership);
        await _context.SaveChangesAsync();
        return Ok(organization);



    }


}


