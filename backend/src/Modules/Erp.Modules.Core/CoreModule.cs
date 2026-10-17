using Erp.Modules.Core.Data;
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

        return services;
    }
}
