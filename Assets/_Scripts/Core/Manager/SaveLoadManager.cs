using System;
using AjaxNguyen.Core.Service;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class SaveLoadManager : PersistentSingleton<SaveLoadManager>
    {
        private const string DATA_FILE_NAME = "FlappyData";

        public GameData gameData;
        JsonDataService dataService;

        public event EventHandler<GameData> OnGameDataChanged;

        protected override void Awake()
        {
            base.Awake();

            gameData = new();
            dataService = new();
        }

        private void Start()
        {
            // ResourceManager.Instance.OnResourceDataChanged += UpdateResourceData;
            LoadGameData();
            OnGameDataChanged?.Invoke(this, gameData);
        }


        private void OnDestroy()
        {
            SaveGameData();
        }

        private void UpdateResourceData(object sender, ResourceData e)
        {
            gameData.resourceData = e;
        }

        public void SaveGameData() => dataService.Save(gameData);

        public void LoadGameData() => gameData = dataService.Load(DATA_FILE_NAME);

        public void CallInvoke() => OnGameDataChanged?.Invoke(this, gameData);

        public void AddListenerForDataChange(EventHandler<GameData> listener)
        {
            OnGameDataChanged += listener;
            // Gọi lại ngay lập tức để cập nhật UI với dữ liệu hiện tại
            OnGameDataChanged.Invoke(this, gameData);
        }

    }
}