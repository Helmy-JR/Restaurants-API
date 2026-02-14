using Restaurants.Domain.Exceptions;

namespace Restaurants.APIs.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(
                ILogger<ErrorHandlingMiddleware> logger
            )
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException NotFound)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(NotFound.Message);

                _logger.LogWarning(NotFound.Message);
            }
            catch (ForbidException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Forbidden");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = "An unexpected error occurred."
                });
            }
        }
    }
}