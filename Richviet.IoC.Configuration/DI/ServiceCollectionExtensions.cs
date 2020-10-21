using AutoMapper;
using Richviet.Services;
using Richviet.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Richviet.Tools.Utility;

namespace Richviet.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IAuthService, FacebookAuthService>();
                services.AddSingleton(new JwtHandler(configuration));
            }
        }

        public static void ConfigureMappings(this IServiceCollection services)
        {
            if (services != null)
            {
                //Automap settings
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
            }
        }
    }
}
