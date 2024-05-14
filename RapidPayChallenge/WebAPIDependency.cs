using Microsoft.Extensions.DependencyInjection;

namespace RapidPayChallenge.WebAPI
{
    public static class WebAPIDependency
    {
        public static IServiceCollection AddWebAPIDependencies(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }
    }
}
