using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using Workgrid.Services;

namespace Workgrid.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        TenantContext tenantContext,
        ApplicationDbContext db)
    {
        var userId = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (userId != null)
        {
            var membership = await db.OrganizationMembers
                .FirstOrDefaultAsync(x =>
                    x.UserId == long.Parse(userId) &&
                    x.IsActive);

            if (membership != null)
            {
                tenantContext.OrganizationId = membership.OrganizationId;
            }
        }

        await _next(context);

       

    }
}
