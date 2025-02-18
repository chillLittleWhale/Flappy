using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AjaxNguyen.Core.Service;
using AjaxNguyen.Utility;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

        JsonDataService jsonDataService;


        protected override void Awake()
        {
            base.Awake();

            resourceData = new();
            skinData = new();
            mapData = new();

            jsonDataService = new();
        }

        public void StartSetUpData(bool isUsingCloudData)
        {
            Debug.Log("StartSetUpData");
            StartCoroutine(SetUpDataCoroutine(isUsingCloudData));
        }

        private IEnumerator SetUpDataCoroutine(bool isUsingCloudData)
        {
            var task = SetUpData(isUsingCloudData);
            while (!task.IsCompleted)
            {
                yield return null; // Đợi đến khi Task hoàn thành
            }
        }


        public async Task SetUpData(bool isUsingCloudData)
        {
            if (isUsingCloudData)
            {
                Debug.Log("Seting up using cloud data ...");
                await SetUpUsingCloudData();
            }
            else
            {
                Debug.Log("Seting up using local data ...");
                SetUpUsingLocalData();
            }
        }

        private void SetUpUsingLocalData()
        {
            LoadAllGameData_Local();

            // Set data thủ công cho các chill manager  vì lần đầu lấy dữ liệu thì các manager chưa kịp đăng ký
            //TODO: cho hết vào 1 hàm cho gọn
            ResourceManager.Instance.SetData(resourceData);
            SkinManager.Instance.SetData(skinData);
            MapManager.Instance.SetData(mapData);
        }

        private async Task SetUpUsingCloudData()
        {
            await LoadAllGameData_Cloud();

            TrySaveData_Local(resourceData, FILE_NAME_RESOURCE);
            TrySaveData_Local(skinData, FILE_NAME_SKIN);
            TrySaveData_Local(mapData, FILE_NAME_MAP);

            SetUpUsingLocalData();
        }

        public void LoadAllGameData_Local()
        {
            Debug.Log("Local data loading ...");
            resourceData = jsonDataService.Load<ResourceData>(FILE_NAME_RESOURCE);
            skinData = jsonDataService.Load<SkinDataJson>(FILE_NAME_SKIN); //TODO
            mapData = jsonDataService.Load<MapDataJson>(FILE_NAME_MAP);
        }

        async Task LoadAllGameData_Cloud()
        {
            try
            {
                var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { FILE_NAME_RESOURCE, FILE_NAME_SKIN, FILE_NAME_MAP });

                if (loadData.TryGetValue("ResourceData", out var resourceItem))
                {
                    resourceData = resourceItem.Value.GetAs<ResourceData>();
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_RESOURCE}' not found.");
                }

                if (loadData.TryGetValue("SkinData", out var skinItem))
                {
                    skinData = skinItem.Value.GetAs<SkinDataJson>();
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_SKIN}' not found.");
                }

                if (loadData.TryGetValue("MapData", out var mapItem))
                {
                    mapData = mapItem.Value.GetAs<MapDataJson>();
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_MAP}' not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading data from Cloud Save: {ex.Message}");
            }
        }

        async Task TrysaveData_Cloud<T>(string fileName)
        {
            T iData = jsonDataService.Load<T>(fileName);

            var saveData = new Dictionary<string, object>
            {
                { fileName, iData }
            };

            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
                Debug.Log($"CloudSave: save {fileName} SUCCESS");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"CloudSave: save {fileName} FAIL: {e.Message}");
            }
        }

        public bool TrySaveData_Local<T>(T data, string fileName)
        {
            if (jsonDataService.Save(data, fileName)) return true; // save success

            Debug.LogWarning($"LocalSave: save {fileName} FAIL");
            return false;
        }

    }

}

// PlayerInfo playerInfo = await AuthenticationService.Instance.GetPlayerInfoAsync();
// Debug.Log("Player info: ");
// Debug.Log("id: " + playerInfo.Id);
// Debug.Log("playerInfo.Username: " + playerInfo.Username);
// Debug.Log("creation time: " + playerInfo.CreatedAt);