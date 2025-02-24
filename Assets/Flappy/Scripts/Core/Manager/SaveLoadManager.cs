using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AjaxNguyen.Utility.Service;
using AjaxNguyen.Event;
using AjaxNguyen.Utility;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Flappy.Core.Manager
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

        [SerializeField] EmptyEventChanel onFinishSetUpData;

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

            onFinishSetUpData.Raise(new Empty());
        }

        private async Task SetUpUsingCloudData()
        {
            await LoadAllGameData_Cloud();

            TrySaveAllData_Local();

            SetUpUsingLocalData();
        }

        public void LoadAllGameData_Local()
        {
            Debug.Log("Local data loading ...");
            resourceData = jsonDataService.Load<ResourceData>(FILE_NAME_RESOURCE);
            skinData = jsonDataService.Load<SkinDataJson>(FILE_NAME_SKIN);
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

        public void ResetAllData_Local()
        {
            try
            {
                string rawData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "DefaultData.json"));

                var defaultJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawData);
                Debug.Log($"defaultJson: {defaultJson}");

                // Ghi đè ResourceData.json 
                if (defaultJson.ContainsKey("ResourceData"))
                {
                    var tempData = JsonConvert.DeserializeObject<ResourceData>(defaultJson["ResourceData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_RESOURCE);
                    Debug.Log("ResourceData.json đã được reset!");
                }

                // Ghi đè SkinData.json 
                if (defaultJson.ContainsKey("SkinData"))
                {
                    var tempData = JsonConvert.DeserializeObject<SkinDataJson>(defaultJson["SkinData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_SKIN);
                    Debug.Log("SkinData.json đã được reset!");
                }

                // Ghi đè MapData.json 
                if (defaultJson.ContainsKey("MapData"))
                {
                    var tempData = JsonConvert.DeserializeObject<SkinDataJson>(defaultJson["MapData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_MAP);
                    Debug.Log("SkinData.json đã được reset!");
                }

            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Lỗi khi reset dữ liệu: {ex.Message}");
            }
        }

        bool TrySaveAllData_Local()
        {
            return (
                TrySaveData_Local(resourceData, FILE_NAME_RESOURCE) &&
                TrySaveData_Local(skinData, FILE_NAME_SKIN) &&
                TrySaveData_Local(mapData, FILE_NAME_MAP)
            );
        }

        async Task<bool> TrySaveAllData_Cloud()
        {
            return (
                await TrysaveData_Cloud<ResourceData>(FILE_NAME_RESOURCE) &&
                await TrysaveData_Cloud<SkinDataJson>(FILE_NAME_SKIN) &&
                await TrysaveData_Cloud<MapDataJson>(FILE_NAME_MAP)
            );
        }

        async Task<bool> TrysaveData_Cloud<T>(string fileName)
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
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"CloudSave: save {fileName} FAIL: {e.Message}");
                return false;
            }
        }

        public bool TrySaveData_Local<T>(T data, string fileName)
        {
            if (jsonDataService.Save(data, fileName)) return true; // save success

            Debug.LogWarning($"LocalSave: save {fileName} FAIL");
            return false;
        }

        public async void HandleUserLogout()
        {
            bool saveSuccess = await TrySaveAllData_Cloud();

            if (saveSuccess)
            {
                ResetAllData_Local();
            }
            else
            {
                Debug.LogError("Lưu dữ liệu lên cloud thất bại! Không reset local data.");
            }
        }

    }
}