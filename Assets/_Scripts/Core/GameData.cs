using System;
using AjaxNguyen.Core.Manager;
using UnityEngine;

namespace AjaxNguyen.Core
{
    [Serializable]
    public class GameData 
    {
        public string fileName = "FlappyData";
        public ResourceData resourceData;

        public GameData()
        {
            resourceData = new ResourceData();
        }
    }
}
