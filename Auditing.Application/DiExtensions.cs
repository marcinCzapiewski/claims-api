using Claims.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing.Application;
public static class DiExtensions
{
    public static IServiceCollection AddAuditing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuditContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        services.AddTransient<IAuditer, Auditer>();

        return services;
    }
}
