using System;
using AjaxNguyen.Core.Manager;
using UnityEngine;

namespace AjaxNguyen.Core
{
    [Serializable]
    // public class GameData 
    // {
    //     public string fileName = "FlappyData";
    //     public ResourceData resourceData;

    //     public GameData()
    //     {
    //         resourceData = new ResourceData();
    //     }
    // }
        public class GameData // không cần nữa, dùng các data riêng
    {
        public string fileName = "FlappyData";
        public ResourceData resourceData;

        public GameData()
        {
            resourceData = new ResourceData();
        }
    }
}
