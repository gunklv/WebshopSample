﻿using Cart.Api.Infrastructure.Integrations.Messaging.Kafka.Abstractions.Consumer.Models;

namespace Cart.Api.Infrastructure.Integrations.Messaging.Kafka.IntegrationEventConsumer.Validators.Abstractions
{
    internal interface IMessageValidatorService<in TMessage> where TMessage : KafkaConsumeMessage
    {
        Task EnsureValidityAsync(TMessage message);
    }
}
