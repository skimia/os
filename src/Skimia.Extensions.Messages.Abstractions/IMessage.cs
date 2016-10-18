using System;

namespace Skimia.Extensions.Messages.Abstractions
{
    public interface IMessage
    {
        event Action<IMessage> Dispatched;

        bool Canceled { get; set; }

        MessagePriority Priority { get; }

        bool IsDispatched { get; }

        void BlockProgression();

        bool Wait();

        bool Wait(TimeSpan timeout);

        /// <summary>
        /// Internal use only
        /// </summary>
        void OnDispatched();
    }
}
