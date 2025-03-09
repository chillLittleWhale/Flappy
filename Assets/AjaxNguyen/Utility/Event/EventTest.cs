using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AjaxNguyen.Utility.Event
{
    // public class EventTest : MonoBehaviour  // test, vô cùng tiềm năng
    // {
    //     private EventSystem eventSystem = new EventSystem();

    //     void Start()
    //     {
    //         // Đăng ký các listener
    //         eventSystem.Subscribe("TestEvent", SyncListener);              // Đồng bộ
    //         eventSystem.Subscribe("TestEvent", AsyncListener);            // Bất đồng bộ
    //         eventSystem.Subscribe("TestEvent", AnotherAsyncListener);     // Bất đồng bộ khác

    //     }

    //     void Update()
    //     {
    //         // on A key press, raise test event
    //         if (Input.GetKeyDown(KeyCode.B)) TriggerEvent();
    //     }

    //     // Listener đồng bộ
    //     private Task SyncListener()
    //     {
    //         Debug.Log("Sync listener executed");
    //         return Task.CompletedTask; // Trả về Task hoàn tất ngay
    //     }

    //     // Listener bất đồng bộ 1
    //     private async Task AsyncListener()
    //     {
    //         Debug.Log("Async listener 1 started");
    //         await Task.Delay(1000); // Giả lập công việc bất đồng bộ
    //         Debug.Log("Async listener 1 executed after 1 second");
    //     }

    //     // Listener bất đồng bộ 2
    //     private async Task AnotherAsyncListener()
    //     {
    //         Debug.Log("Async listener 2 started");
    //         await Task.Delay(2000); // Giả lập công việc lâu hơn
    //         Debug.Log("Async listener 2 executed after 2 seconds");
    //     }

    //     // Kích hoạt sự kiện với callback
    //     private async void TriggerEvent()
    //     {
    //         await eventSystem.RaiseEventAsync("TestEvent", async () =>
    //         {
    //             Debug.Log("Callback executed after all listeners completed");
    //             await Task.Delay(1000);
    //             Debug.Log("Callback executed after all listeners completed after 1 second");
    //         });
    //     }
    // }

}
