using System;
using AjaxNguyen.Core.Service;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    // public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    // {
    //     private const string DATA_FILE_NAME = "FlappyData";

    //     public GameData gameData;
    //     JsonDataService dataService;

    //     public event EventHandler<GameData> OnGameDataChanged;

    //     protected override void Awake()
    //     {
    //         base.Awake();

    //         gameData = new();
    //         dataService = new();
    //     }

    //     private void Start()
    //     {
    //         // ResourceManager.Instance.OnResourceDataChanged += UpdateResourceData;
    //         LoadGameData();
    //         OnGameDataChanged?.Invoke(this, gameData);
    //     }


    //     private void OnDestroy()
    //     {
    //         SaveGameData();
    //     }

    //     private void UpdateResourceData(object sender, ResourceData e)
    //     {
    //         gameData.resourceData = e;
    //     }

    //     public void SaveGameData() => dataService.Save(gameData);

    //     public void LoadGameData() => gameData = dataService.Load(DATA_FILE_NAME);

    //     public void CallInvoke() => OnGameDataChanged?.Invoke(this, gameData);

    //     // public void AddListenerForDataChange(EventHandler<GameData> listener)
    //     // {
    //     //     OnGameDataChanged += listener;
    //     //     // Gọi lại ngay lập tức để cập nhật UI với dữ liệu hiện tại
    //     //     OnGameDataChanged.Invoke(this, gameData);
    //     // }

    // }


    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        private const string FILE_NAME_RESOURCE = "ResourceData";

        public ResourceData resourceData;

        JsonDataService dataService;

        public event EventHandler<ResourceData> OnResourceDataChanged;

        protected override void Awake()
        {
            base.Awake();

            resourceData = new();
            dataService = new();
        }

        private void Start()
        {
            LoadAllGameData(); // load all data

            OnResourceDataChanged?.Invoke(this, resourceData);
            
            // Set data to all chill manager  vì lần đầu lấy dữ liệu thì các manager chưa kịp đăng ký
            ResourceManager.Instance.SetData(resourceData);

        }


        // private void OnDestroy()
        // {
        //     SaveGameData();
        // }

        // private void UpdateResourceData(object sender, ResourceData e)
        // {
        //     resourceData = e;
        // }

        // public void SaveGameData() // save từng cụm data
        // {
        //     dataService.Save(resourceData, FILE_NAME_RESOURCE);
        // }

        public void LoadAllGameData()  // load lần lượt từng cụm data
        {
            resourceData = dataService.Load<ResourceData>(FILE_NAME_RESOURCE);
        }

        public void TrySaveResourceData(ResourceData data) 
        {
            if(dataService.Save(data, FILE_NAME_RESOURCE)) // save success
            {
                resourceData = data;
                OnResourceDataChanged?.Invoke(this, resourceData);
            }
        }
        public void CallInvoke()
        {
            OnResourceDataChanged?.Invoke(this, resourceData);
        }
    }

}