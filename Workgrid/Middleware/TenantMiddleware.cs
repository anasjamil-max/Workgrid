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
      TenantContext tenantContext)
    {
        await _next(context);
    }
}
