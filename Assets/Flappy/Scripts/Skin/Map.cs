using System;
using UnityEngine;

namespace Flappy.Core
{
    [Serializable]
    public class Map
    {
        public string id;
        public string mapName;
        public Sprite mapIcon;
        public string mapMusicName;
        public int unlockCost;
        public bool isUnlocked;

        public Map(string id, string name, Sprite icon, string mapMusicName, int cost, bool isUnlocked)
        {
            this.id = id;
            this.mapName = name;
            this.mapIcon = icon;
            this.mapMusicName = mapMusicName;
            this.unlockCost = cost;
            this.isUnlocked = isUnlocked;
        }

        public Map() { }

    }

    [Serializable]
    public class MapJson
    {
        public string id;
        public bool isUnlocked;

        public MapJson(string id, bool isUnlocked)
        {
            this.id = id;
            this.isUnlocked = isUnlocked;
        }
    }
}
