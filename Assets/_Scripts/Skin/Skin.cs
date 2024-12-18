using System;
using UnityEngine;

namespace AjaxNguyen.Core
{
    [Serializable]
    public class Skin 
    {
        public string id;
        public string skinName;
        public Sprite skinIcon;
        public int unlockCost;
        public bool isUnlocked;

        public Skin(string id, string name, Sprite icon, int cost, bool isUnlocked)
        {
            this.id = id;
            this.skinName = name;
            this.skinIcon = icon;
            this.unlockCost = cost;
            this.isUnlocked = isUnlocked;
        }

        public Skin(){
        }
    }
}
