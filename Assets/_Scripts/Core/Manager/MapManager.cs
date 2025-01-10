using System;
using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class MapManager : PersistentSingleton<MapManager>
    {
        public event EventHandler<MapData> OnMapDataChanged;
        public MapData data;
        private MapData tempData;
        [SerializeField] MapDataJson tempDataJson;

        public List<GameObject> mapPrefabs; // Gán từ Inspector
        private GameObject currentMap;

        // void Start()
        // {
        //      SaveLoadManager.Instance.TrySaveMapData(tempDataJson); // DO NOT DELETE: đoạn này để đẩy dữ liệu thủ công vào json.
        // }

        // private void UpdateMapData(object sender, MapData e)
        // {
        //     data = e;
        // }

        public void LoadMap()
        {
            var mapPrefab = mapPrefabs.Find(m => m.name.Contains(data.selectedMapID)); 
            if (mapPrefab != null)
            {
                if (currentMap != null) Destroy(currentMap); // Xóa map cũ
                currentMap = Instantiate(mapPrefab);
            }
        }


        public void UnlockMap(string mapId)
        {
            tempData = new MapData(data);
            tempData.mapList.Find(map => map.id == mapId).isUnlocked = true;
            TrySaveData(tempData);
        }

        public void ReSelectMap(string mapId)
        {
            tempData = new MapData(data);
            tempData.selectedMapID = mapId;
            TrySaveData(tempData);
        }

        public void SetData(MapDataJson newData)
        {
            tempDataJson = newData;

            data.selectedMapID = newData.selectedMapID;

            foreach (var map in newData.mapList)
            {
                map.isUnlocked = newData.mapList.Find(m => m.id == map.id)?.isUnlocked ?? false;
            }
        }

        private void TrySaveData(MapData data)
        {
            tempDataJson = ToSkinDataJson(data);

            if (SaveLoadManager.Instance.TrySaveMapData(tempDataJson))
            {
                this.data = data;
                OnMapDataChanged?.Invoke(this, data);
            }
            else Debug.LogWarning("Save resource data fail");
        }

        private MapDataJson ToSkinDataJson(MapData data)
        {
            MapDataJson jsonData = new MapDataJson
            {
                selectedMapID = data.selectedMapID,
                mapList = new List<MapJson>()
            };

            foreach (Map map in data.mapList)
            {
                jsonData.mapList.Add(new MapJson(map.id, map.isUnlocked));
            }

            return jsonData;
        }
    }

    [Serializable]
    public class MapData
    {
        public List<Map> mapList;
        public string selectedMapID;

        // parameterless constructor for generic types
        public MapData()
        {
            mapList = new List<Map>();
            selectedMapID = "0";
        }

        //copy constructor
        public MapData(MapData data)
        {
            mapList = new List<Map>(data.mapList);
        }
    }

    [Serializable]
    public class MapDataJson
    {
        public List<MapJson> mapList;
        public string selectedMapID;

        // parameterless constructor for generic types
        public MapDataJson()
        {
            mapList = new List<MapJson>();
            selectedMapID = "0";
        }

        //copy constructor
        public MapDataJson(MapDataJson data)
        {
            mapList = new List<MapJson>(data.mapList);
        }
    }
}

