using Microsoft.EntityFrameworkCore;
using Workgrid.Data;
using Workgrid.Services;
var builder = WebApplication.CreateBuilder(args);
//db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddScoped<TenantContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
