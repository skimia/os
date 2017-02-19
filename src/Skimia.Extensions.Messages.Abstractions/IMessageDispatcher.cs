namespace Skimia.Extensions.Messages.Abstractions
{
    public interface IMessageDispatcher
    {
        void ProcessDispatching(object processor);
        void Wait();
    }
}
