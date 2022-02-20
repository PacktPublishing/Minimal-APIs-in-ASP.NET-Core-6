using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chapter08.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Description = "Insert the token with the 'Bearer ' prefix"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecuritystring")),
        ValidIssuer = "https://www.packtpub.com",
        ValidAudience = "Minimal APIs Client",
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});

builder.Services.AddAuthorization(options =>
{
    /* Use this code to define a default policy that is automatically applied when using the
     * Authorize attribute or the RequireAuthorization method with no other parameters.
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
      .RequireClaim("tenant-id").Build();

    options.DefaultPolicy = policy;
    */

    /* Setting the FallbackPolicy equals to DefaultPolicy, means that every endpoints is protected,
     * even if we don't specify the Authorize attribute or the RequireAuthorization method.
     */
    options.FallbackPolicy = options.DefaultPolicy;

    options.AddPolicy("Tenant42", policy =>
    {
        policy.RequireClaim("tenant-id", "42");
    });

    options.AddPolicy("TimedAccessPolicy", policy =>
    {
        policy.Requirements.Add(new MaintenanceTimeRequirement
        {
            StartTime = new TimeOnly(0, 0, 0),
            EndTime = new TimeOnly(4, 0, 0)
        });
    });
});

builder.Services.AddScoped<IAuthorizationHandler, MaintenanceTimeAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// We use AllowAnonymous to bypass the FallbackPolicy, that requires an authenticated user.
app.MapPost("/api/auth/login", [AllowAnonymous] (LoginRequest request) =>
{
    if (request.Username == "marco" && request.Password == "P@$$w0rd")
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, "Administrator"),
            new(ClaimTypes.Role, "User"),
            new("tenant-id", "42")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecuritystring"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: "https://www.packtpub.com",
            audience: "Minimal APIs Client",
            claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Results.Ok(new { AccessToken = accessToken });
    }

    return Results.BadRequest();
});

app.MapGet("/api/attribute-protected", [Authorize] () => "This endpoint is protected using the Authorize attribute");

app.MapGet("/api/method-protected", () => "This endpoint is protected using the RequireAuthorization method")
.RequireAuthorization();

app.MapGet("/api/me", [Authorize] (ClaimsPrincipal user) => $"Logged username: {user.Identity!.Name}");

app.MapGet("/api/admin-attribute-protected", [Authorize(Roles = "Administrator")] () => { });

app.MapGet("/api/admin-method-protected", () => { })
.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrator" });

app.MapGet("/api/stackeholder-protected", [Authorize(Roles = "Stakeholder")] () => { });

app.MapGet("/api/role-check", [Authorize] (ClaimsPrincipal user) =>
{
    if (user.IsInRole("Administrator"))
    {
        return "User is an Administrator";
    }

    return "This is a normal user";
});

app.MapGet("/api/claim-attribute-protected", [Authorize(Policy = "Tenant42")] () => { });

app.MapGet("/api/claim-method-protected", () => { })
.RequireAuthorization("Tenant42");

app.MapGet("/api/custom-policy-protected", [Authorize(Policy = "TimedAccessPolicy")] () => { });

app.Run();

public record LoginRequest(string Username, string Password);