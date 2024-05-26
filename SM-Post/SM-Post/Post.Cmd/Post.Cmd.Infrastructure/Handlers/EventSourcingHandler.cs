using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Handlers
{
    internal class EventSourcingHandler : IEventSourceHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var agreggate = new PostAggregate();

            var events = await _eventStore.GetEventAsync(aggregateId);
            if(events==null &&!events.Any()) {
                return agreggate;
            }
            agreggate.ReplayEvent(events);

            agreggate.Version= events.Select(e => e.Version).Max();
            return agreggate;

        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUnCommitedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}
