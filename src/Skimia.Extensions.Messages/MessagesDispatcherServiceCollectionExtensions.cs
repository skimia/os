using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.Messages.Abstractions;

namespace Skimia.Extensions.Messages
{
    public static class MessagesDispatcherServiceCollectionExtensions
    {
        public static void AddMessages(this IServiceCollection services)
        {
            var messageDispatcher = new MessageDispatcher();
            var dispatcherTask = new MessageDispatcherTask(messageDispatcher);

            services.AddSingleton<IMessageDispatcher>(messageDispatcher);
            services.AddSingleton<MessageDispatcherTask>(dispatcherTask);//TODO interface for dispatchertask
        }
    }
}
