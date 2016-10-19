using Microsoft.AspNetCore.Builder;
using Skimia.Extensions.Messages.Abstractions;

namespace Skimia.Extensions.Messages
{
    public static class DispatcherTaskApplicationBuilderExtensions
    {
        public static void UseMessages(this IApplicationBuilder services)
        {
            var dispatcherTask = (MessageDispatcherTask)services.ApplicationServices.GetService(typeof(MessageDispatcherTask));

            dispatcherTask.Start();
        }
    }
}
