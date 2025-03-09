using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AjaxNguyen
{
    public class AsyncEventBus   // test, khả năng là không dùng
    {
        private static readonly Dictionary<Type, List<object>> _handlers = new();

        public static void Subscribe<T>(Func<T, Task> handler)
        {
            if (!_handlers.ContainsKey(typeof(T)))
                _handlers[typeof(T)] = new List<object>();
            _handlers[typeof(T)].Add(handler);
        }

        public static async Task Publish<T>(T message)
        {
            if (_handlers.TryGetValue(typeof(T), out var handlers))
            {
                var tasks = handlers.Cast<Func<T, Task>>().Select(h => h(message));
                await Task.WhenAll(tasks);
            }
        }
    }

    public struct AsyncEBusTest { }
}
