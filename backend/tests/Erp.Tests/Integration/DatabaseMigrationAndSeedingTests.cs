using Erp.Modules.Core.Data;
using Erp.Modules.TelePharmacy.Data;
using Erp.SharedKernel.Interfaces;
using Erp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Erp.Tests.Integration;

[Collection("IntegrationTests")]
public class DatabaseMigrationAndSeedingTests
{
    private readonly ErpTestWebApplicationFactory _factory;

    public DatabaseMigrationAndSeedingTests(ErpTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Database_SchemasAndIdempotentSeeding_ExecuteWithoutErrors()
    {
        using var scope = _factory.Services.CreateScope();
        var coreDb = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        var telePharmDb = scope.ServiceProvider.GetRequiredService<TelePharmacyDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // Verify migrations applied and schemas accessible
        Assert.True(await coreDb.Database.CanConnectAsync());
        Assert.True(await telePharmDb.Database.CanConnectAsync());

        // Run idempotent seeding a second time to ensure zero unique constraint crashes
        await CoreDbSeeder.SeedAsync(coreDb, passwordHasher);
        await TelePharmacyDbSeeder.SeedAsync(telePharmDb);

        // Verify seeded data exists in isolated schemas
        var seededTenant = await coreDb.Tenants.AsNoTracking().FirstOrDefaultAsync(t => t.Id == CoreDbSeeder.DefaultTenantId);
        Assert.NotNull(seededTenant);

        var seededPharmacistProfile = await telePharmDb.PharmacistProfiles.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == TelePharmacyDbSeeder.DefaultPharmacistProfileId);
        Assert.NotNull(seededPharmacistProfile);

        var herbsCount = await telePharmDb.HerbsCatalog.AsNoTracking().CountAsync();
        Assert.True(herbsCount >= 5);
    }
}
