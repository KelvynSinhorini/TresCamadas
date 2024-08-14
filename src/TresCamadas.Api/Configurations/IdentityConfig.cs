using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TresCamadas.Api.Data;

namespace TresCamadas.Api.Configurations;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfig(this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
