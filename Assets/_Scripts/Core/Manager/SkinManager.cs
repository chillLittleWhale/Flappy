using System;
using System.Collections.Generic;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class SkinManager : PersistentSingleton<SkinManager>
    {
        public event EventHandler<SkinData> OnSkinDataChanged;
        public SkinData data;
        private SkinData tempData;

        void Start()
        {
            // SaveLoadManager.Instance.OnSkinDataChanged +=  UpdateSkinData;

            // SaveLoadManager.Instance.TrySaveSkinData(data); // DO NOT DELETE: đoạn này để đẩy dữ liệu thủ công vào json.
        }

        private void UpdateSkinData(object sender, SkinData e)
        {
            data = e;
        }

        public void UnlockSkin(string skinId)
        {
            tempData = new SkinData(data);

            tempData.skinList.Find(skin => skin.id == skinId).isUnlocked = true;

            TrySaveData(tempData);
        }

        public void SetData(SkinData data)
        {
            this.data = data;
        }

        private void TrySaveData(SkinData data)
        {
            if (SaveLoadManager.Instance.TrySaveSkinData(data))
            {
                this.data = data;
                OnSkinDataChanged?.Invoke(this, data);
            }
            else Debug.LogWarning("Save resource data fail");
        }
    }

    [Serializable]
    public class SkinData
    {
        public List<Skin> skinList;
        public string selectedSkinID;

        // parameterless constructor for generic types
        public SkinData()
        {
            skinList = new List<Skin>();
            selectedSkinID = "0";
        }

        //copy constructor
        public SkinData(SkinData data)
        {
            skinList = new List<Skin>(data.skinList);
        }
    }
}
