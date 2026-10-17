using Erp.Modules.TelePharmacy.Data;
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

        return services;
    }
}
