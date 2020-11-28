using System;
using Frontend.DB.EF.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Command.UseCase;
using Users.Domains.Users.Query;

namespace DraftUser.Remover
{
    public static class RemoveDraftUserFunction
    {
        [FunctionName("RemoveDraftUser-Function")]
        public static void Run([TimerTrigger("0 0 0 1 * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var contextOptions = new DbContextOptionsBuilder<GeneralContext>()
            .UseSqlServer(System.Environment.GetEnvironmentVariable("general_connection"))
            .Options;
            var generalContext = new GeneralContext(contextOptions);
            IUserQueryRepositories queryRepositories = new UserQueryRepositories(generalContext);
            IUserCommandRepository commandRepository = new UserCommandDbCommandRepository(generalContext);

            DraftUserRemover remover = new DraftUserRemover(commandRepository, queryRepositories);
            var days = System.Environment.GetEnvironmentVariable("expirydDys");

            int effectiveNumberOfRecord =  remover.Execute(int.Parse(days));

        }
    }
}
