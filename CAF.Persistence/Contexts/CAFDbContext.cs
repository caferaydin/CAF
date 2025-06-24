using System.Reflection;
using CAF.Application.Abstractions.Services.Session;
using CAF.Domain.Entities.Authentication;
using CAF.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Contexts
{
    public class CAFDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly ICurrentUserSession _currentUserSession;

        public CAFDbContext(DbContextOptions<CAFDbContext> options, ICurrentUserSession currentUserSession) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            _currentUserSession = currentUserSession;
        }


        #region Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyEntityAudit();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyEntityAudit()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserSession.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModificationDate = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserSession.UserName;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.DeletionDate = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        entry.Entity.DeletedBy = _currentUserSession.UserName;
                        entry.Property(x => x.DeletionDate).CurrentValue = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
        #endregion
    }
}
