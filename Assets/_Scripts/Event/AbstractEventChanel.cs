using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AjaxNguyen.Event
{
    public abstract class AbstractEventChanel<T> : ScriptableObject
    {
        readonly HashSet<AbstractEventListener<T>> listeners = new();

        public void RegisterListener(AbstractEventListener<T> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(AbstractEventListener<T> listener)
        {
            listeners.Remove(listener);
        }

        public void Raise(T value)
        {
            foreach (var listener in listeners)
            {
                listener.Raise(value);
            }
        }
    }
}
