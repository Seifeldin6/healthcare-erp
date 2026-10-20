using System.Text;
using System.Text.Json.Serialization;
using Erp.Api.Services;
using Erp.Modules.Core;
using Erp.Modules.Core.Data;
using Erp.Modules.TelePharmacy;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Hubs;
using Erp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// JSON Enum Serialization for Minimal APIs & Swagger Schema Generation
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Swagger & OpenAPI with JWT Bearer support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Healthcare ERP API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter your token below."
    });

    c.AddSecurityRequirement(doc =>
    {
        var requirement = new OpenApiSecurityRequirement();
        requirement.Add(new OpenApiSecuritySchemeReference("Bearer", doc, null), new List<string>());
        return requirement;
    });
});

// JWT Bearer authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is required.");

if (jwtKey.Length < 32)
    throw new InvalidOperationException("Jwt:Key must be at least 32 characters long.");

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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
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
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Healthcare ERP API v1");
        c.RoutePrefix = "swagger";
    });

    // Idempotent migration verification and data seeding in development
    using var scope = app.Services.CreateScope();
    var coreDb = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
    var telePharmDb = scope.ServiceProvider.GetRequiredService<TelePharmacyDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

    await coreDb.Database.MigrateAsync();
    await telePharmDb.Database.MigrateAsync();

    await CoreDbSeeder.SeedAsync(coreDb, passwordHasher);
    await TelePharmacyDbSeeder.SeedAsync(telePharmDb);
}

app.UseAuthentication();
app.UseAuthorization();

// Map SignalR hubs
app.MapHub<ConsultationHub>("/hubs/consultation");

// Map module endpoints
app.MapCoreEndpoints();
app.MapTelePharmacyEndpoints();

app.Run();

public partial class Program { }
