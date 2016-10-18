using System;

namespace Skimia.Extensions.Messages.Abstractions
{
    public interface IMessageHandler
    {
        object Container { get; }
        Type ContainerType { get; }
        Type MessageType { get; }
        MessageHandlerAttribute Attribute { get; }
        Action<object, object, IMessage> Action { get; }
        Type TokenType { get; }
    }
}
