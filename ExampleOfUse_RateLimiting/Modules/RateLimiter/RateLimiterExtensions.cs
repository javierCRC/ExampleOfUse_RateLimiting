using Microsoft.AspNetCore.RateLimiting; // middleware provides rate limiting middleware. Apps configure rate limiting policies and then attach the policies to endpoints.

namespace ExampleOfUse_RateLimiting.Modules.RateLimiter
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            var vFixedWindowsPolicy = "fixedWindows";

            services.AddRateLimiter(configureOptions => // to add a rate limiting service to the service collection
            {
                configureOptions.AddFixedWindowLimiter(policyName: vFixedWindowsPolicy, fixedWindow => // to create a fixed window limiter with a policy name of "fixed" and sets:
                {
                    fixedWindow.PermitLimit = int.Parse(configuration["RateLimiting:vPermitLimit"]); // A maximum of 5 requests
                    fixedWindow.Window = TimeSpan.FromSeconds(int.Parse(configuration["RateLimiting:vWindow"])); //  per each 30-second window are allowed.
                    fixedWindow.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    fixedWindow.QueueLimit = int.Parse(configuration["RateLimiting:vQueueLimit"]); // Maximum cumulative permit count of queued acquisition requests.

                });

                // YOU CAN ADD MORE policy here

                configureOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // The HTTP 429 Too Many Requests response status code indicates the user has sent too many requests in a given amount of time ("rate limiting").
            });

            return services;
        }
    }
}

// This is the official documentation https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-7.0