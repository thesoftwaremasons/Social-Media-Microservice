using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Event
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AggregatorIdentifier { get; set; }  
        public string AggregatorType { get; set;}
        public int Version { get; set;}
        public string EventType { get; set;}
        public BaseEvent EventData { get; set; }
    }
}
