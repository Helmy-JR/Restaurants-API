using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using Restaurants.APIs.Middlewares;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Sevices
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Host.UseSerilog((context, configuration)=>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

        #endregion

        ;
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
        app.UseAuthorization();
        app.MapControllers();

        #endregion

        app.Run();
    }
}