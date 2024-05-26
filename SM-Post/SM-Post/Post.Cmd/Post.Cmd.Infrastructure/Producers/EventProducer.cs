using Confluent.Kafka;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        public EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;
            //var testConfig = new ProducerConfig
            //{
            //    BootstrapServers = "localhost:9902"
            //};
        }

        public async Task ProducerAsync<T>(string topic, T @event) where T : class
        {
            using var producer = new ProducerBuilder<string, string>(_config)
                                     .SetKeySerializer(Serializers.Utf8)
                                     .SetValueSerializer(Serializers.Utf8)
                                     .Build();
            var eventMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };
            var deleveryResult = await producer.ProduceAsync(topic, eventMessage);
            if (deleveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reasons: {deleveryResult.Message} .");
            }

        }
    }
}
