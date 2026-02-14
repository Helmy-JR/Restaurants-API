using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using Restaurants.APIs.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.APIs.Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Sevices
        // Add services to the container.
        
        builder.AddPresentation();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        #endregion

        var app = builder.Build();

        // Data Seeding
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
        await seeder.SeedAsync();

        #region Configure Kestrel middlewares
        // Configure the HTTP request pipeline.
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<RequestTimeLoggingMiddleware>();
        
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.MapGroup("api/auth")
           .WithTags("Identity")
           .MapIdentityApi<User>();
        
        app.UseAuthorization();
        app.MapControllers();

        #endregion

        app.Run();
    }
}