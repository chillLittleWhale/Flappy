using System;
using Flappy.Core.Manager;
using UnityEngine;

namespace Flappy.Core
{
    [Serializable]
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
