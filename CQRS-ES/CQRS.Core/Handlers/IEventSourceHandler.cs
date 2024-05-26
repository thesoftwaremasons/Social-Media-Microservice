﻿using CQRS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Handlers
{
    public interface IEventSourceHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregateRoot);
        Task<T> GetByIdAsync(Guid aggregateId);
    }
}
