using AutoMapper;
using Richviet.Services;
using Richviet.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Richviet.Tools.Utility;
using Richviet.Services.Services;

namespace Richviet.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                services.AddSingleton(new JwtHandler(configuration));
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IAuthService, FacebookAuthService>();
                services.AddScoped<IBankService, BankService>();
                services.AddScoped<IPayeeRelationService, PayeeRalationService>();
                services.AddScoped<IBeneficiarService, BeneficiarService>();
                services.AddScoped<IPayeeTypeService, PayeeTypeService>();
                services.AddSingleton(typeof(FolderHandler));
                services.AddSingleton<IUploadPic, UploadPicToLocalService>();
                services.AddScoped<ICurrencyService,CurrencyService>();
                services.AddScoped<IExchangeRateService, DBExchangeRateService>();
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
