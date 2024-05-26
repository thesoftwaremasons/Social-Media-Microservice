using CQRS.Core.Domain;
using CQRS.Core.Event;
using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Store
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;

        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventStoreRepository = eventStoreRepository;
            _eventProducer = eventProducer;
        }
        
        public async Task<List<BaseEvent>> GetEventAsync(Guid aggregateId)
        {
            var eventStream= await _eventStoreRepository.FindByAggregateId(aggregateId);
            if(eventStream==null || !eventStream.Any())
            {
                throw new AggregateNotFoundException("Incorrect post ID provided");
            }
            return eventStream.OrderBy(a => a.Version).Select(a => a.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = await GetEventAsync(aggregateId);
            //eventStream[^1].Version != expectedVersion also mean eventStream[-1].Version != expectedVersion

            //also trying to check concurrency control
            if (expectedVersion !=-1 && eventStream[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }
            int version = expectedVersion;
            foreach (var @event in eventStream)
            {
                version++;
                @event.Version=version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregatorIdentifier=aggregateId,
                    AggregatorType=nameof(PostAggregate),
                    Version=version,
                    EventType=eventType,
                    EventData=@event
                    
                };
                await _eventStoreRepository.SaveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProducerAsync(topic, @event);
            }
        }
    }
}
