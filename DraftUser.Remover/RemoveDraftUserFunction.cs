using System;
using Frontend.DB.EF.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Users.Domains.Users.Command.Adapter.Repositories;
using Users.Domains.Users.Command.UseCase;
using Users.Domains.Users.Query;

namespace DraftUser.Remover
{
    public static class RemoveDraftUserFunction
    {
        [FunctionName("RemoveDraftUser-Function")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var generalContext = new GeneralContext();
            IUserQueryRepositories queryRepositories = new UserQueryRepositories(generalContext);
            IUserCommandRepository commandRepository = new UserCommandDbCommandRepository(generalContext);

            DraftUserRemover remover = new DraftUserRemover(commandRepository, queryRepositories);

            int effectiveNumberOfRecord =  remover.Execute();

        }
    }
}
