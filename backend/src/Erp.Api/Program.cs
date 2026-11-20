using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Erp.Api.Middleware;
using Erp.Api.Services;
using Erp.Modules.Core;
using Erp.Modules.Core.Data;
using Erp.Modules.TelePharmacy;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Hubs;
using Erp.SharedKernel.Interfaces;
using Erp.SharedKernel.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

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
            },

            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var error = ErrorResponse.Unauthorized(
                    "Your session has expired. Please sign in again.",
                    code: "SESSION_EXPIRED",
                    traceId: context.HttpContext.TraceIdentifier);

                await context.Response.WriteAsJsonAsync(
                    error,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var error = ErrorResponse.Forbidden(
                    "You do not have permission to perform this action.",
                    code: "FORBIDDEN",
                    traceId: context.HttpContext.TraceIdentifier);

                await context.Response.WriteAsJsonAsync(
                    error,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PatientOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Patient"));

    options.AddPolicy("PharmacistOnly", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Pharmacist"));

    options.AddPolicy("ClinicalStaff", policy =>
        policy.RequireAuthenticatedUser().RequireRole("Pharmacist", "Doctor"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services
    .AddCoreModule(builder.Configuration)
    .AddTelePharmacyModule(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();

app.UseExceptionHandler();

app.UseStatusCodePages(async statusCodeContext =>
{
    var context = statusCodeContext.HttpContext;
    if (context.Request.Path.StartsWithSegments("/hubs"))
        return;

    var response = context.Response;
    if (response.HasStarted || response.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) == true)
        return;

    var traceId = context.TraceIdentifier;
    var statusCode = response.StatusCode;

    var error = statusCode switch
    {
        StatusCodes.Status400BadRequest => ErrorResponse.BadRequest("The submitted request is invalid.", code: "BAD_REQUEST", traceId: traceId),
        StatusCodes.Status401Unauthorized => ErrorResponse.Unauthorized("Your session has expired. Please sign in again.", code: "SESSION_EXPIRED", traceId: traceId),
        StatusCodes.Status403Forbidden => ErrorResponse.Forbidden("You do not have permission to perform this action.", code: "FORBIDDEN", traceId: traceId),
        StatusCodes.Status404NotFound => ErrorResponse.NotFound("The requested resource was not found.", code: "RESOURCE_NOT_FOUND", traceId: traceId),
        StatusCodes.Status405MethodNotAllowed => new ErrorResponse(false, StatusCodes.Status405MethodNotAllowed, "METHOD_NOT_ALLOWED", "The requested HTTP method is not allowed for this endpoint.", null, traceId),
        StatusCodes.Status415UnsupportedMediaType => new ErrorResponse(false, StatusCodes.Status415UnsupportedMediaType, "UNSUPPORTED_MEDIA_TYPE", "The submitted media format is unsupported. Please use application/json.", null, traceId),
        _ => new ErrorResponse(false, statusCode, "INTERNAL_ERROR", "Something went wrong on our end. Please try again later.", null, traceId)
    };

    response.ContentType = "application/json";
    await response.WriteAsJsonAsync(
        error,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
});

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Healthcare ERP API v1");
        c.RoutePrefix = "swagger";
    });

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

app.MapHub<ConsultationHub>("/hubs/consultation");

app.MapCoreEndpoints();
app.MapTelePharmacyEndpoints();

app.Run();

public partial class Program { }
