namespace ModelBuilder
{
    using NServiceBus.Model.Domain;

    public class InMemoryModelBuilder
    {
        private readonly Model model = new Model();

        public void Accept(ProcessedMessage processedMessage)
        {
            if (!CanAccept(processedMessage))
            {
                return;
            }

            var message = GetInterimMessage(processedMessage);

            var sendingEndpoint = model.GetEndpoint(processedMessage.SendingEndpoint.Name);
            sendingEndpoint.Sends(message);

            var receivingEndpoint = model.GetEndpoint(processedMessage.ReceivingEndpoint.Name);
            receivingEndpoint.Receives(message);

            if (!string.IsNullOrEmpty(processedMessage.Context))
            {
                var context = model.GetContext(processedMessage.Context);
                context.Contains(message);
            }
        }

        private InterimMessage GetInterimMessage(ProcessedMessage processedMessage)
        {
            var message = model.GetInterimMessage(processedMessage.MessageId);
            message.Type = processedMessage.MessageType;
            message.Intent = processedMessage.MessageIntent;

            if (processedMessage.RelatedTo.HasValue)
            {
                message.RelatedTo = processedMessage.RelatedTo.Value;
            }

            return message;
        }

        public Model GetModel()
        {
            model.Compact();
            return model;
        }

        private static bool CanAccept(ProcessedMessage message)
        {
            if (message.IsSystemMessage)
            {
                return false;
            }

            var intent = message.MessageIntent;
            if (string.IsNullOrEmpty(message.MessageIntent))
            {
                return false;
            }

            if (intent == "Subscribe" || intent == "Unsubscribe")
            {
                return false;
            }

            return true;
        }
    }
}