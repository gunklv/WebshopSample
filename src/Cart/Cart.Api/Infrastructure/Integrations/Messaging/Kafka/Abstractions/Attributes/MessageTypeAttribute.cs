namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypeAttribute : Attribute
    {
        public string MessageType { get; private set; }

        public MessageTypeAttribute(string messageType)
        {
            MessageType = messageType;
        }
    }
}
