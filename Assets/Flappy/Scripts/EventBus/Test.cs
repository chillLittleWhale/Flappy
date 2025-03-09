using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace AjaxNguyen
{
    public class Test : MonoBehaviour
    {
        EventBinding<TestEvent> testEventBinding;

        void OnEnable()
        {
            testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
            EventBus<TestEvent>.Register(testEventBinding);
        }

        void OnDisable()
        {
            EventBus<TestEvent>.Deregister(testEventBinding);
        }

        async void HandleTestEvent(TestEvent testEvent)
        {
            await Task.Delay(111);
            Debug.LogWarning("CCCCCCCCCCCCCCC");
        }

        void Update()
        {
            // on A key press, raise test event
            if (Input.GetKeyDown(KeyCode.A)) EventBus<TestEvent>.Raise(new TestEvent());
        }
    }
}
