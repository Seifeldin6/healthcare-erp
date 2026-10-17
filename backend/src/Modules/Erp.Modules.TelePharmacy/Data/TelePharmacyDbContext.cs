using Erp.Modules.TelePharmacy.Data.Configurations;
using Erp.Modules.TelePharmacy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Erp.Modules.TelePharmacy.Data;

public class TelePharmacyDbContext : DbContext
{
    public TelePharmacyDbContext(DbContextOptions<TelePharmacyDbContext> options) : base(options) { }

    public DbSet<PharmacistProfile> PharmacistProfiles => Set<PharmacistProfile>();
    public DbSet<HerbsCatalog> HerbsCatalog => Set<HerbsCatalog>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<VideoSession> VideoSessions => Set<VideoSession>();
    public DbSet<HerbalPrescription> HerbalPrescriptions => Set<HerbalPrescription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("telepharmacy");

        modelBuilder.ApplyConfiguration(new PharmacistProfileConfiguration());
        modelBuilder.ApplyConfiguration(new HerbsCatalogConfiguration());
        modelBuilder.ApplyConfiguration(new ConsultationConfiguration());
        modelBuilder.ApplyConfiguration(new ChatRoomConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
        modelBuilder.ApplyConfiguration(new VideoSessionConfiguration());
        modelBuilder.ApplyConfiguration(new HerbalPrescriptionConfiguration());
    }
}
