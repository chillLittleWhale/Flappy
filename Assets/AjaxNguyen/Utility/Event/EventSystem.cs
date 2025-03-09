using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AjaxNguyen.Utility.Event
{
    public class EventSystem
    {
        private static readonly EventSystem _instance = new EventSystem();
        public static EventSystem Instance => _instance;

        // Danh sách các listener cho mỗi sự kiện (key là tên sự kiện)
        private readonly Dictionary<string, List<Func<Task>>> _listeners = new();

        // Đăng ký listener (có thể là đồng bộ hoặc bất đồng bộ)
        public void Subscribe(string eventName, Func<Task> listener)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] = new List<Func<Task>>();
            }
            _listeners[eventName].Add(listener);
            Debug.Log($"Listener registered for event: {eventName}");
        }

        // Hủy đăng ký listener
        public void Unsubscribe(string eventName, Func<Task> listener)
        {
            if (_listeners.ContainsKey(eventName))
            {
                _listeners[eventName].Remove(listener);
                Debug.Log($"Listener unregistered for event: {eventName}");
            }
        }

        // Kích hoạt sự kiện và chờ tất cả listener hoàn tất, sau đó gọi callback
        public async Task RaiseEventAsync(string eventName, Action callback = null)
        {
            if (!_listeners.ContainsKey(eventName) || _listeners[eventName].Count == 0)
            {
                Debug.LogWarning($"No listeners for event: {eventName}");
                callback?.Invoke();
                return;
            }

            Debug.Log($"Raising event: {eventName}");
            var listeners = _listeners[eventName].ToArray(); // Sao chép để tránh thay đổi trong khi chạy
            var tasks = listeners.Select(listener => listener()).ToArray();

            // Chờ tất cả listener hoàn tất
            await Task.WhenAll(tasks);

            Debug.Log($"All listeners for event {eventName} completed");
            callback?.Invoke();
        }
    }

    // public class EventSystem
    // {
    //     // Singleton instance
    //     private static readonly EventSystem _instance = new EventSystem();
    //     public static EventSystem Instance => _instance;

    //     // Danh sách các listener cho mỗi sự kiện
    //     private readonly Dictionary<string, List<object>> _listeners = new();

    //     // Constructor riêng tư để ngăn tạo instance từ bên ngoài
    //     private EventSystem() { }

    //     // Đăng ký listener với dữ liệu generic
    //     public void Subscribe<T>(string eventName, Func<T, Task> listener)
    //     {
    //         if (!_listeners.ContainsKey(eventName))
    //         {
    //             _listeners[eventName] = new List<object>();
    //         }
    //         _listeners[eventName].Add(listener);
    //         Debug.Log($"Listener registered for event: {eventName}");
    //     }

    //     // Hủy đăng ký listener
    //     public void Unsubscribe<T>(string eventName, Func<T, Task> listener)
    //     {
    //         if (_listeners.ContainsKey(eventName))
    //         {
    //             _listeners[eventName].Remove(listener);
    //             Debug.Log($"Listener unregistered for event: {eventName}");
    //         }
    //     }

    //     // Kích hoạt sự kiện với dữ liệu và chờ tất cả listener hoàn tất, sau đó gọi callback
    //     public async Task RaiseEventAsync<T>(string eventName, T eventData, Action callback = null)
    //     {
    //         if (!_listeners.ContainsKey(eventName) || _listeners[eventName].Count == 0)
    //         {
    //             Debug.LogWarning($"No listeners for event: {eventName}");
    //             callback?.Invoke();
    //             return;
    //         }

    //         Debug.Log($"Raising event: {eventName} with data: {eventData}");
    //         var listeners = _listeners[eventName].ToArray(); // Sao chép để tránh thay đổi trong khi chạy
    //         var tasks = listeners.Select(listener => ((Func<T, Task>)listener)(eventData)).ToArray();

    //         // Chờ tất cả listener hoàn tất
    //         await Task.WhenAll(tasks);

    //         Debug.Log($"All listeners for event {eventName} completed");
    //         callback?.Invoke();
    //     }
    // }


    // public class EventSystem
    // {
    //     private static readonly EventSystem _instance = new EventSystem();
    //     public static EventSystem Instance => _instance;

    //     // Danh sách listener không dữ liệu (Action hoặc Func<Task>)
    //     private readonly Dictionary<string, List<object>> _noDataListeners = new();
    //     // Danh sách listener có dữ liệu (Action<T> hoặc Func<T, Task>)
    //     private readonly Dictionary<(string, Type), List<object>> _dataListeners = new();

    //     private EventSystem() { }

    //     // Đăng ký listener không dữ liệu (đồng bộ)
    //     public void Subscribe(string eventName, Action listener)
    //     {
    //         if (!_noDataListeners.ContainsKey(eventName))
    //         {
    //             _noDataListeners[eventName] = new List<object>();
    //         }
    //         _noDataListeners[eventName].Add(listener);
    //         Debug.Log($"No-data Action listener registered for event: {eventName}");
    //     }

    //     // Đăng ký listener không dữ liệu (bất đồng bộ)
    //     public void Subscribe(string eventName, Func<Task> listener)
    //     {
    //         if (!_noDataListeners.ContainsKey(eventName))
    //         {
    //             _noDataListeners[eventName] = new List<object>();
    //         }
    //         _noDataListeners[eventName].Add(listener);
    //         Debug.Log($"No-data Task listener registered for event: {eventName}");
    //     }

    //     // Đăng ký listener có dữ liệu (đồng bộ)
    //     public void Subscribe<T>(string eventName, Action<T> listener)
    //     {
    //         var key = (eventName, typeof(T));
    //         if (!_dataListeners.ContainsKey(key))
    //         {
    //             _dataListeners[key] = new List<object>();
    //         }
    //         _dataListeners[key].Add(listener);
    //         Debug.Log($"Data Action listener registered for event: {eventName} with type {typeof(T)}");
    //     }

    //     // Đăng ký listener có dữ liệu (bất đồng bộ)
    //     public void Subscribe<T>(string eventName, Func<T, Task> listener)
    //     {
    //         var key = (eventName, typeof(T));
    //         if (!_dataListeners.ContainsKey(key))
    //         {
    //             _dataListeners[key] = new List<object>();
    //         }
    //         _dataListeners[key].Add(listener);
    //         Debug.Log($"Data Task listener registered for event: {eventName} with type {typeof(T)}");
    //     }

    //     // Kích hoạt sự kiện không dữ liệu
    //     public async Task RaiseEventAsync(string eventName, Action callback = null)
    //     {
    //         if (!_noDataListeners.ContainsKey(eventName) || _noDataListeners[eventName].Count == 0)
    //         {
    //             Debug.LogWarning($"No listeners for event: {eventName}");
    //             callback?.Invoke();
    //             return;
    //         }

    //         Debug.Log($"Raising no-data event: {eventName}");
    //         var listeners = _noDataListeners[eventName].ToArray();
    //         var tasks = new List<Task>();

    //         foreach (var listener in listeners)
    //         {
    //             if (listener is Action action)
    //             {
    //                 action();
    //             }
    //             else if (listener is Func<Task> taskFunc)
    //             {
    //                 tasks.Add(taskFunc());
    //             }
    //         }

    //         await Task.WhenAll(tasks);
    //         Debug.Log($"All no-data listeners for event {eventName} completed");
    //         callback?.Invoke();
    //     }

    //     // Kích hoạt sự kiện có dữ liệu
    //     public async Task RaiseEventAsync<T>(string eventName, T eventData, Action callback = null)
    //     {
    //         var key = (eventName, typeof(T));
    //         if (!_dataListeners.ContainsKey(key) || _dataListeners[key].Count == 0)
    //         {
    //             Debug.LogWarning($"No listeners for event: {eventName} with type {typeof(T)}");
    //             callback?.Invoke();
    //             return;
    //         }

    //         Debug.Log($"Raising data event: {eventName} with data: {eventData}");
    //         var listeners = _dataListeners[key].ToArray();
    //         var tasks = new List<Task>();

    //         foreach (var listener in listeners)
    //         {
    //             if (listener is Action<T> action)
    //             {
    //                 action(eventData);
    //             }
    //             else if (listener is Func<T, Task> taskFunc)
    //             {
    //                 tasks.Add(taskFunc(eventData));
    //             }
    //         }

    //         await Task.WhenAll(tasks);
    //         Debug.Log($"All data listeners for event {eventName} completed");
    //         callback?.Invoke();
    //     }
    // }

   public struct EmptyEventData { }
}
