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
using Unity.Services.Authentication;
using AjaxNguyen.Utility.Event;

namespace Flappy.Core.Manager
{
    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        public const string FILE_NAME_RESOURCE = "ResourceData";
        public const string FILE_NAME_SKIN = "SkinData";
        public const string FILE_NAME_MAP = "MapData";
        public const string FILE_NAME_PLAYER_INFO = "PlayerInfoData";
        public const string FILE_NAME_DAILY_REWARD = "DailyRewardData";

        public ResourceData resourceData;
        public SkinDataJson skinData;
        public MapDataJson mapData;
        public PlayerInfoData playerInfoData;
        public DailyRewardData dailyRewardData;

        JsonDataService jsonDataService;

        [SerializeField] EmptyEventChanel onFinishSetUpData;

        protected override void Awake()
        {
            base.Awake();

            resourceData = new();
            skinData = new();
            mapData = new();
            playerInfoData = new();
            dailyRewardData = new();

            jsonDataService = new();
        }

        void Start()
        {
            // Đăng ký các listener
            EventSystem.Instance.Subscribe("SignOutEvent", HandleUserLogout);
            EventSystem.Instance.Subscribe("SignUpEvent", TrySaveAllData_Cloud);
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
            PlayerInfoManager.Instance.SetData(playerInfoData);
            DailyRewardManager.Instance.SetData(dailyRewardData);

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
            playerInfoData = jsonDataService.Load<PlayerInfoData>(FILE_NAME_PLAYER_INFO);
            dailyRewardData = jsonDataService.Load<DailyRewardData>(FILE_NAME_DAILY_REWARD);
        }

        async Task LoadAllGameData_Cloud()
        {
            try
            {
                var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { FILE_NAME_RESOURCE, FILE_NAME_SKIN, FILE_NAME_MAP, FILE_NAME_PLAYER_INFO, FILE_NAME_DAILY_REWARD });

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

                if (loadData.TryGetValue("PlayerInfoData", out var playerInfoItem))
                {
                    playerInfoData = playerInfoItem.Value.GetAs<PlayerInfoData>();
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_PLAYER_INFO}' not found.");
                }

                if (loadData.TryGetValue("DailyRewardData", out var dailyRewardItem))
                {
                    dailyRewardData = dailyRewardItem.Value.GetAs<DailyRewardData>();
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_DAILY_REWARD}' not found.");
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
                    var tempData = JsonConvert.DeserializeObject<MapDataJson>(defaultJson["MapData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_MAP);
                    Debug.Log("MapData.json đã được reset!");
                }

                // Ghi đè PlayerInfoData.json 
                if (defaultJson.ContainsKey("PlayerInfoData"))
                {
                    var tempData = JsonConvert.DeserializeObject<PlayerInfoData>(defaultJson["PlayerInfoData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_PLAYER_INFO);
                    Debug.Log("PlayerInfoData.json đã được reset!");
                }

                // Ghi đè DailyRewardData.json 
                if (defaultJson.ContainsKey("DailyRewardData"))
                {
                    var tempData = JsonConvert.DeserializeObject<DailyRewardData>(defaultJson["DailyRewardData"].ToString());
                    jsonDataService.Save(tempData, FILE_NAME_DAILY_REWARD);
                    Debug.Log("DailyRewardData.json đã được reset!");
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
                TrySaveData_Local(mapData, FILE_NAME_MAP) &&
                TrySaveData_Local(playerInfoData, FILE_NAME_PLAYER_INFO) &&
                TrySaveData_Local(dailyRewardData, FILE_NAME_DAILY_REWARD)
            );
        }

        async Task<bool> TrySaveAllData_Cloud()
        {
            return (
                await TrysaveData_Cloud<ResourceData>(FILE_NAME_RESOURCE) &&
                await TrysaveData_Cloud<SkinDataJson>(FILE_NAME_SKIN) &&
                await TrysaveData_Cloud<MapDataJson>(FILE_NAME_MAP) &&
                await TrysaveData_Cloud<PlayerInfoData>(FILE_NAME_PLAYER_INFO) &&
                await TrysaveData_Cloud<DailyRewardData>(FILE_NAME_DAILY_REWARD)
            );
        }

        private async Task<bool> TrysaveData_Cloud<T>(string fileName)
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

        public async Task<bool>  TrysaveData_Both<T>(T data,string fileName)
        {
            var saveData = new Dictionary<string, object>
            {
                { fileName, data }
            };

            try
            {
                await CloudSaveService.Instance.Data.Player.SaveAsync(saveData);
                Debug.Log($"CloudSave: save {fileName} SUCCESS");
                if (TrySaveData_Local(data, fileName))
                {
                    return true;
                }
                return false;
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

        public async Task HandleUserLogout() //  void
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