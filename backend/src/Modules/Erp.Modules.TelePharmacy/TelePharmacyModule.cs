using Erp.Modules.TelePharmacy.Adapters;
using Erp.Modules.TelePharmacy.Data;
using Erp.Modules.TelePharmacy.Endpoints;
using Erp.Modules.TelePharmacy.Hubs;
using Erp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Erp.Modules.TelePharmacy;

public static class TelePharmacyModule
{
    public static IServiceCollection AddTelePharmacyModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TelePharmacyDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "telepharmacy")));

        services.AddScoped<IVideoProvider, JitsiVideoProvider>();

        services.AddSignalR();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TelePharmacyModule).Assembly));

        return services;
    }

    public static IEndpointRouteBuilder MapTelePharmacyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapHerbsEndpoints();
        app.MapPharmacistEndpoints();
        app.MapConsultationEndpoints();
        return app;
    }
}
