using Erp.Modules.Core.Data;
using Erp.Modules.Core.Endpoints;
using Erp.Modules.Core.Gateways;
using Erp.Modules.Core.Services;
using Erp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Modules.Core;

public static class CoreModule
{
    public static IServiceCollection AddCoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CoreDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "core")));

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<JwtTokenGenerator>();
        services.AddScoped<IPaymentGateway, MockPaymentGateway>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CoreModule).Assembly));

        return services;
    }

    public static IEndpointRouteBuilder MapCoreEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapPatientEndpoints();
        app.MapPaymentEndpoints();
        return app;
    }
}
