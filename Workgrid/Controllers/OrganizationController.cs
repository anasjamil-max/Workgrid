using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workgrid.DTOs.Organization;
using Workgrid.Models;
using Workgrid.Data;
using Microsoft.EntityFrameworkCore;

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



    //get

    [HttpGet]
    

    public async Task<IActionResult> GetMyOrganizations()
    {
        var userId = long.Parse(
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        );


        var organizations = await _context.OrganizationMembers
           .Where(x => x.UserId == userId && x.IsActive)

           .Join(

            _context.Organizations,
            member => member.OrganizationId,
            organization => organization.Id,
            (member, organization) => organization


            )

           .ToListAsync();
        return Ok(organizations);
             
    }


    [HttpPost("invite")]
    public async Task<IActionResult> InviteUser(InviteUserRequest request)
    {
        var userId = long.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == request.OrganizationId &&
                x.UserId == userId &&
                x.IsActive);

        if (membership == null || membership.Role != "Owner")
        {
            return Forbid();
        }

        var invitedUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (invitedUser == null)
        {
            return NotFound("User not found.");
        }

        var existingMembership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.OrganizationId == request.OrganizationId &&
                x.UserId == invitedUser.Id);

        if (existingMembership != null)
        {
            return BadRequest("User is already a member.");
        }

        var invitation = new Invitation
        {
            OrganizationId = request.OrganizationId,
            InvitedByUserId = userId,
            Email = request.Email,
            Role = request.Role,
            Token = Guid.NewGuid().ToString(),
            Status = "Pending",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();

        return Ok(invitation);
    }






}


    




