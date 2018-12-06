namespace Exporter
{
    using System;

    using NServiceBus.Model.Data;

    public interface IMessageProcessor
    {
        void RegisterListener(Action<ProcessedMessage> onNext);
    }
}