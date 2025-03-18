using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Flappy.Core
{
    [Serializable]
    public class Skin 
    {
        public string id;
        public string skinName;
        public Sprite skinIcon;
        public SpriteLibraryAsset libraryAsset;
        public string description;
        public int fallbackGold;  // nếu gacha trùng thì hoàn lại 1 lượng vàng
        public bool isUnlocked;

        public Skin(string id, string name, Sprite icon, string description, int fallbackGold, bool isUnlocked)
        {
            this.id = id;
            this.skinName = name;
            this.skinIcon = icon;
            this.description = description;
            this.fallbackGold = fallbackGold;
            this.isUnlocked = isUnlocked;
        }

        public Skin(){
        }
    }

    [Serializable]
    public class SkinJson 
    {
        public string id;
        public bool isUnlocked;

        public SkinJson(string id, bool isUnlocked)
        {
            this.id = id;
            this.isUnlocked = isUnlocked;
        }
    }
}
