using System.Text;
using Erp.Api.Services;
using Erp.Modules.Core;
using Erp.Modules.TelePharmacy;
using Erp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// JWT Bearer authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is required.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer is required.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience is required.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Patient"));

    options.AddPolicy("PharmacistOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Pharmacist"));

    options.AddPolicy("ClinicalStaff", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Pharmacist", "Doctor"));
});

// HTTP context accessor for ICurrentUserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Modules
builder.Services
    .AddCoreModule(builder.Configuration)
    .AddTelePharmacyModule(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map module endpoints
app.MapCoreEndpoints();
app.MapTelePharmacyEndpoints();

app.Run();
