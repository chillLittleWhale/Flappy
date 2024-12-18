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

        public ResourceData resourceData;
        public SkinData skinData;

        JsonDataService dataService;

        // public event EventHandler<ResourceData> OnResourceDataChanged; //--
        public event EventHandler<SkinData> OnSkinDataChanged; //--


        protected override void Awake()
        {
            base.Awake();

            resourceData = new();
            skinData = new();

            dataService = new();
        }

        private void Start()
        {
            LoadAllGameData();

            // OnResourceDataChanged?.Invoke(this, resourceData); //--
            OnSkinDataChanged?.Invoke(this, skinData);

            // Set data thủ công cho các chill manager  vì lần đầu lấy dữ liệu thì các manager chưa kịp đăng ký
            //TODO: cho hết vào 1 hàm cho gọn
            ResourceManager.Instance.SetData(resourceData);
            SkinManager.Instance.SetData(skinData);

        }

        public void LoadAllGameData()
        {
            resourceData = dataService.Load<ResourceData>(FILE_NAME_RESOURCE);
            skinData = dataService.Load<SkinData>(FILE_NAME_SKIN); //TODO
        }

        // public void TrySaveResourceData(ResourceData data)  // TODO: đổi return type của các hàm TrySave thành bool để các manager xử lý
        // {
        //     if (dataService.Save(data, FILE_NAME_RESOURCE)) // save success
        //     {
        //         resourceData = data;
        //         OnResourceDataChanged?.Invoke(this, resourceData);
        //     }
        //     else
        //     {
        //         Debug.LogWarning("Save resource data fail");
        //     }
        // }
        public bool TrySaveResourceData(ResourceData data)
        {
            if (dataService.Save(data, FILE_NAME_RESOURCE)) // save success
            {
                resourceData = data;
                // OnResourceDataChanged?.Invoke(this, resourceData);
                return true;
            }

            Debug.LogWarning("Save resource data fail");
            return false;
        }

        public bool TrySaveSkinData(SkinData data)
        {
            if (dataService.Save(data, FILE_NAME_SKIN))
            {
                skinData = data;
                // OnSkinDataChanged?.Invoke(this, skinData);
                return true;
            }
            
            return false;
        }
        // public bool TrySaveSkinData(SkinData data)
        // {
        //     return dataService.Save(data, FILE_NAME_SKIN);
        // }

        // public void CallInvoke()  // sử dụng bởi các UI
        // {
        //     // OnResourceDataChanged?.Invoke(this, resourceData); //--
        // }
    }

}