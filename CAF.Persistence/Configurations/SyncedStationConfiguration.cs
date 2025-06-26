using Kronos.StationModule.Domain.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CAF.Persistence.Configurations;

public class SyncedStationConfiguration : IEntityTypeConfiguration<SyncedStation>
{
    public void Configure(EntityTypeBuilder<SyncedStation> builder)
    {

        builder.ToTable("KRONOS_STATION");

        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.StationCode)
            .IsUnique();

        builder.Property(s => s.StationName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(s => s.SetupDate)
            .IsRequired();
        builder.Property(s => s.Platform)
            .IsRequired();
        builder.Property(s => s.ServiceTypes)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(s => s.Source)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(s => s.LastSynced)
            .IsRequired();
    }
}
