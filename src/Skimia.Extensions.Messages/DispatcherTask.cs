using System.Threading.Tasks;
using Skimia.Extensions.Messages.Abstractions;

namespace Skimia.Extensions.Messages
{
    public class MessageDispatcherTask
    {
        public IMessageDispatcher Dispatcher { get; }

        public bool Running
        {
            get;
            private set;
        }

        public object Processor { get; }

        public MessageDispatcherTask(IMessageDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            Processor = this;
        }

        public MessageDispatcherTask(IMessageDispatcher dispatcher, object processor)
        {
            Dispatcher = dispatcher;
            Processor = processor;
        }

        public void Start()
        {
            Running = true;
            Task.Factory.StartNew(Process);
        }

        public void Stop()
        {
            Running = false;
        }

        private void Process()
        {
            while (Running)
            {
                Dispatcher.Wait();

                if (Running)
                    Dispatcher.ProcessDispatching(Processor);
            }
        }
    }
}
