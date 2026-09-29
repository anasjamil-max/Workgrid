using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;
using Workgrid.Data;
using Workgrid.Middleware;
using Workgrid.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
var databaseUrl = builder.Configuration["DATABASE_URL"];

var databaseUri = new Uri(databaseUrl!);

var userInfo = databaseUri.UserInfo.Split(':');

var username = Uri.UnescapeDataString(userInfo[0]);
var password = Uri.UnescapeDataString(userInfo[1]);

var connectionString =
    $"Host={databaseUri.Host};" +
    $"Port={databaseUri.Port};" +
    $"Database={databaseUri.AbsolutePath.TrimStart('/')};" +
    $"Username={username};" +
    $"Password={password};" +
    $"SSL Mode=Require;Trust Server Certificate=true;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Services
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthorizationService>();

builder.Services.AddControllers();

// Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});
// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger

    app.UseSwagger();
    app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}




app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();