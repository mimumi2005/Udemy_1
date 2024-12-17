
using System.Diagnostics;

namespace Restaurants.API.Middleware
{
    public class TimerMiddleware(ILogger<TimerMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var timer = Stopwatch.StartNew();
            await next.Invoke(context);
            timer.Stop();
            if (timer.ElapsedMilliseconds > 4000)
            {
                logger.LogWarning("Request [{Verb}] at {Path} took {Time} ms", context.Request.Method, context.Request.Path, timer.ElapsedMilliseconds);
            }
        }
    }
}
