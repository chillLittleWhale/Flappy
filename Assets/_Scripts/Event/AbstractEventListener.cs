using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AjaxNguyen.Event
{
    public abstract class AbstractEventListener<T> : MonoBehaviour
    {
        [SerializeField] AbstractEventChanel<T> eventChanel;
        [SerializeField] UnityEvent<T> unityEvent;

        private void OnEnable()
        {
            eventChanel.RegisterListener(this);
        }

        private void OnDisable()
        {
            eventChanel.UnregisterListener(this);
        }

        public void Raise(T value)
        {
            unityEvent?.Invoke(value);
        }
    }
}
