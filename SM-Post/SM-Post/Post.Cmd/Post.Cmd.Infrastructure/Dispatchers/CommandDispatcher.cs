﻿using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();
        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {

            if (_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException("you cannot register the same command twice");
            }
            _handlers.Add(typeof(T), x => handler((T)x));
            throw new NotImplementedException();
        }

        public async Task SendAsync(BaseCommand command)
        {
            if (_handlers.TryGetValue(command.GetType(),out Func<BaseCommand, Task> handler))
            {
                await handler(command);
            }
            {
                throw new ArgumentNullException(nameof(handler),"No command handler was found");
            }
        }
    }
}
