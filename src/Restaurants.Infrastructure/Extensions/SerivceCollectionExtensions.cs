using Restaurants.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Authorization.Services;


namespace Restaurants.Infrastructure.Extensions
{
    public static class SerivceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add infrastructure related services here
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<RestaurantsDbContext>(options => options.UseMySql(connectionString, 
                new MySqlServerVersion(new Version(8, 0, 26))));

            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalfactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IDishesRepository, DishesRepository>();

            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.HasNationality, 
                    policy => policy.RequireClaim(AppClaimTypes.Nationality/* ,"German","Polish" //limits the access to users with german or polish nationality*/))
                .AddPolicy(PolicyNames.AtLeast20,
                    policy => policy.AddRequirements(new MinimumAgeRequirement(20)))
                .AddPolicy(PolicyNames.CreatedAtLeast2Restaurants, 
                    policy => policy.AddRequirements(new CreatedMultipleRestaurantRequirement(2)));

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
            services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantRequirementHandler>();
            

        }
    }
}