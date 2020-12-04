using AutoMapper;
using Richviet.Services;
using Richviet.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Richviet.Tools.Utility;
using Richviet.Services.Services;
using System.Collections.Generic;
using Richviet.BackgroudTask.Arc;
using Richviet.API.Helper;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Command.UseCase;
using Users.Domains.Users.Query;
using RemitRecords.Domains.RemitRecords.Query;
using Email.Notifier;
using Microsoft.Extensions.Options;
using RemitRecords.Domains.RemitRecords.Command.UseCase;
using RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories;

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
                services.AddScoped<IRemitRecordService, RemitRecordService>();
                services.AddScoped<IAuthService, FacebookAuthService>();
                services.AddScoped<IBankService, BankService>();
                services.AddScoped<IPayeeRelationService, PayeeRalationService>();
                services.AddScoped<IBeneficiaryService, BeneficiaryService>();
                services.AddScoped<IPayeeTypeService, PayeeTypeService>();
                services.AddSingleton(typeof(FolderHandler));
                services.AddSingleton(typeof(ArcValidationTask));
                services.AddSingleton<IUploadPic, UploadPicToAzuareBlobService>();
                services.AddScoped<ICurrencyService,CurrencyService>();
                services.AddScoped<IExchangeRateService, DBExchangeRateService>();
                services.AddScoped<IRemitSettingService, RemitSettingService>();
                
                services.AddScoped<IDiscountService,DiscountService>();
                services.AddScoped<IUserAdminService, UserAdminService>();
                services.AddScoped<IUserLoginLogService, UserLoginService>();
                services.AddTransient<IUserCommandRepository, UserCommandDbCommandRepository>();
                services.AddTransient<IUserQueryRepositories, UserQueryRepositories>();
                services.AddTransient<UserModifier>();
                services.AddTransient<UserRemoverForDevUse>();
                services.AddTransient<RemitRecordAmlReviewer>();
                services.AddTransient<RemitTransactionStatusModifier>();
                services.AddTransient<ArcValidationTask>();
                services.AddTransient<IRemitRecordCommandRepository, RemitRecordCommandDbCommandRepository>();
                services.AddScoped<IArcScanRecordService,ArcScanRecordService>();
                services.AddTransient<IFirebaseService, FirebaseService>();
                services.AddTransient<RemitValidationHelper>();
                services.AddScoped<IBannerService,BannerService>();
                services.AddTransient<IRemitRecordQueryRepositories, RemitRecordQueryRepositories>();
                IOptions<SendGridEmailSenderOptions> sendGridoptions = Options.Create(new SendGridEmailSenderOptions()
                {
                    ApiKey = configuration["SendGridKey"],
                    SenderEmail = configuration["SendGridSenderEmail"],
                    SenderName = configuration["SendGridSenderName"]
                });
                services.AddSingleton<IEmailSender>(sender=>new SendGridEmailSender(sendGridoptions));
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
