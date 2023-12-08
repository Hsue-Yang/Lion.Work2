using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.HostedService
{
    internal class SystemLogBackgroundService : BackgroundService
    {
        private readonly ISystmeLogQueues _queues;

        public SystemLogBackgroundService(ISystmeLogQueues queues)
        {
            _queues = queues;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => _queues.Consumer(), cancellationToken);
        }
    }
}
