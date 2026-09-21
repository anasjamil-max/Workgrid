using Microsoft.EntityFrameworkCore;
using Workgrid.Data;

namespace Workgrid.Services;

public class AuthorizationService
{
    private readonly ApplicationDbContext _context;

    public AuthorizationService(ApplicationDbContext context)
    {
        
        
        _context = context;
    }
    public async Task<bool> IsOwnerOrManager( long userId, long organizationId)

    {
        var membership = await _context.OrganizationMembers
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.OrganizationId == organizationId &&
                x.IsActive);

        if (membership == null)
        {
            return false;
        }

        return membership.Role == "Owner" ||
               membership.Role == "Manager";
    }




}