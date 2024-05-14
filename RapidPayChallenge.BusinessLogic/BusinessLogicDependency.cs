using Microsoft.Extensions.DependencyInjection;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Data.Repositories;

namespace RapidPayChallenge.BusinessLogic
{
    public static class BusinessLogicDependency
    {
        public static IServiceCollection AddBusinessLogicDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICardMngrService, CardMngrService>();
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPaymFeeRepository, PaymFeeRepository>();

            return services;
        }
    }
}
