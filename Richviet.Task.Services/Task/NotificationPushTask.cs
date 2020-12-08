using Microsoft.Extensions.Logging;
using Richviet.Task.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Task.Services.Task
{
    public class NotificationPushTask : ITask
    {
        private readonly ILogger logger;

        public NotificationPushTask(ILogger<NotificationPushTask> logger)
        {
            this.logger = logger;
        }

        public void Start()
        {

        }


    }
}
