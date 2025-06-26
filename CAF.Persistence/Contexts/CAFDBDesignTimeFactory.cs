using CAF.Infrastructure.Services.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CAF.Persistence.Contexts
{
    public class CAFDBDesignTimeFactory : IDesignTimeDbContextFactory<CAFDbContext>
    {
        public CAFDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CAFDbContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Database=CAF;User Id=postgres;Password=postgres123;");
            return new CAFDbContext(optionsBuilder.Options, new CurrentUserSession(new HttpContextAccessor()));
        }
    }
}
