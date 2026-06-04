using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Persistence.IoC;

public static class Register
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Also register a factory so services that need a short-lived, independent context
        // (e.g. fire-and-forget background writes) can create one without touching the
        // scoped instance that the current request is already using.
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
    }
}