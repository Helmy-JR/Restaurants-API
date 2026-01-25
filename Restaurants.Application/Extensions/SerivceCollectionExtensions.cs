using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;


namespace Restaurants.Application.Extensions
{
    public static class SerivceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(SerivceCollectionExtensions).Assembly;
            
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(applicationAssembly));

            services.AddAutoMapper(applicationAssembly);

            services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();
        }
    }
}