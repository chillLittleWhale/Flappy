using System;
using AjaxNguyen.Core.Service;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        private const string FILE_NAME_RESOURCE = "ResourceData";
        private const string FILE_NAME_SKIN = "SkinData";
        private const string FILE_NAME_MAP = "MapData";

        public ResourceData resourceData;
        public SkinDataJson skinData;
        public MapDataJson mapData;

        JsonDataService dataService;


        protected override void Awake()
        {
            base.Awake();

            resourceData = new();
            skinData = new();
            mapData = new();

            dataService = new();
        }

        private void Start()
        {
            LoadAllGameData();

            // Set data thủ công cho các chill manager  vì lần đầu lấy dữ liệu thì các manager chưa kịp đăng ký
            //TODO: cho hết vào 1 hàm cho gọn
            ResourceManager.Instance.SetData(resourceData);
            SkinManager.Instance.SetData(skinData);
            MapManager.Instance.SetData(mapData);

        }

        public void LoadAllGameData()
        {
            resourceData = dataService.Load<ResourceData>(FILE_NAME_RESOURCE);
            skinData = dataService.Load<SkinDataJson>(FILE_NAME_SKIN); //TODO
            mapData = dataService.Load<MapDataJson>(FILE_NAME_MAP);
        }

        public bool TrySaveResourceData(ResourceData data)
        {
            if (dataService.Save(data, FILE_NAME_RESOURCE)) // save success
            {
                resourceData = data;
                return true;
            }

            Debug.LogWarning("Save resource data fail");
            return false;
        }

        // public bool TrySaveSkinData(SkinData data)
        // {
        //     if (dataService.Save(data, FILE_NAME_SKIN))
        //     {
        //         skinData = data;
        //         return true;
        //     }

        //     Debug.LogWarning("Save skin data fail");
        //     return false;
        // }

        public bool TrySaveSkinData(SkinDataJson data)
        {
            if (dataService.Save(data, FILE_NAME_SKIN))
            {
                skinData = data;
                return true;
            }

            Debug.LogWarning("Save skin data fail");
            return false;
        }

        public bool TrySaveMapData(MapDataJson data)
        {
            if (dataService.Save(data, FILE_NAME_MAP))
            {
                mapData = data;
                return true;
            }

            Debug.LogWarning("Save map data fail");
            return false;
        }

    }

}