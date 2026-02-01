using System.Diagnostics;

namespace Restaurants.APIs.Middlewares
{
    public class RequestTimeLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeLoggingMiddleware> _logger;

        public RequestTimeLoggingMiddleware(
                ILogger<RequestTimeLoggingMiddleware> logger
            )
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();
            await next.Invoke(context);
            stopWatch.Stop();

            if(stopWatch.ElapsedMilliseconds /1000 > 4)
            {
                _logger.LogWarning("Request [{Verb}] at [{Path}] took {Time} ms.", 
                    context.Request.Method, 
                    context.Request.Path, 
                    stopWatch.ElapsedMilliseconds
                );
            }
        }
    }
}