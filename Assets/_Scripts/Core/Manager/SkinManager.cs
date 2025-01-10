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
        [SerializeField]private SkinDataJson tempDataJson;

        // void Start()
        // {
        //      SaveLoadManager.Instance.TrySaveSkinData(tempDataJson); // DO NOT DELETE: đoạn này để đẩy dữ liệu thủ công vào json.
        // }

        // private void UpdateSkinData(object sender, SkinData e)
        // {
        //     data = e;
        // }

        public void UnlockSkin(string skinId)
        {
            tempData = new SkinData(data);
            tempData.skinList.Find(skin => skin.id == skinId).isUnlocked = true;
            TrySaveData(tempData);
        }

        public void ReSelectSkin(string skinId)
        {
            tempData = new SkinData(data);
            tempData.selectedSkinID = skinId;
            TrySaveData(tempData);
        }

        public void SetData(SkinDataJson newData)
        {
            tempDataJson = newData;

            data.selectedSkinID = newData.selectedSkinID;

            foreach (var skin in data.skinList)
            {
                skin.isUnlocked = newData.skinList.Find(s => s.id == skin.id)?.isUnlocked ?? false;
            }
        }

        // private void TrySaveData(SkinData data)
        // {
        //     if (SaveLoadManager.Instance.TrySaveSkinData(data))
        //     {
        //         this.data = data;
        //         OnSkinDataChanged?.Invoke(this, data);
        //     }
        //     else Debug.LogWarning("Save resource data fail");
        // }

        private void TrySaveData(SkinData data)
        {
            tempDataJson = ToSkinDataJson(data);

            if (SaveLoadManager.Instance.TrySaveSkinData(tempDataJson))
            {
                this.data = data;
                OnSkinDataChanged?.Invoke(this, data);
            }
            else Debug.LogWarning("Save resource data fail");
        }

        private SkinDataJson ToSkinDataJson(SkinData data)
        {
            SkinDataJson jsonData = new SkinDataJson
            {
                selectedSkinID = data.selectedSkinID,
                skinList = new List<SkinJson>()
            };

            foreach (Skin skin in data.skinList)
            {
                jsonData.skinList.Add(new SkinJson(skin.id, skin.isUnlocked));
            }

            return jsonData;
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

    [Serializable]
    public class SkinDataJson
    {
        public List<SkinJson> skinList;
        public string selectedSkinID;

        // parameterless constructor for generic types
        public SkinDataJson()
        {
            skinList = new List<SkinJson>();
            selectedSkinID = "0";
        }

        //copy constructor
        public SkinDataJson(SkinDataJson data)
        {
            skinList = new List<SkinJson>(data.skinList);
        }
    }
}
