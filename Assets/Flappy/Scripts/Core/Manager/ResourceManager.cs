using System;
using AjaxNguyen.Utility;
using UnityEngine;

namespace Flappy.Core.Manager
{
    public class ResourceManager : PersistentSingleton<ResourceManager>
    {
        public event EventHandler<ResourceData> OnResourceDataChanged; //--
        public ResourceData data;

        private ResourceData tempData;  // when save load error occurs, this will make sure the real data is not be effected

        // void Start()
        // {
        //     // SaveLoadManager.Instance.OnResourceDataChanged += UpdateResourceData; //--
        // }

        public void AddResource(ResourceType resourceType, int amount)
        {
            tempData = new ResourceData(data);

            switch (resourceType)
            {
                case ResourceType.Gold:
                    tempData.gold += amount;
                    break;
                case ResourceType.Diamond:
                    tempData.diamond += amount;
                    break;
                case ResourceType.Stamina:
                    tempData.stamina += amount;
                    break;
                default:
                    Debug.LogWarning("AddResource: Unknown resource type: " + resourceType);
                    break;
            }

            TrySaveData(tempData);
        }

        public void SpendResource(ResourceType resourceType, int amount)
        {
            tempData = new ResourceData(data);

            switch (resourceType)
            {
                case ResourceType.Gold:
                    if (tempData.gold < amount) return;
                    tempData.gold -= amount;
                    break;
                case ResourceType.Diamond:
                    if (tempData.diamond < amount) return;
                    tempData.diamond -= amount;
                    break;
                case ResourceType.Stamina:
                    if (tempData.stamina < amount) return;
                    tempData.stamina -= amount;
                    break;
                default:
                    Debug.LogWarning("SpendResource: Unknown resource type: " + resourceType);
                    break;
            }

            TrySaveData(tempData);
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Gold:
                    return data.gold;
                case ResourceType.Diamond:
                    return data.diamond;
                case ResourceType.Stamina:
                    return data.stamina;
                default:
                    Debug.LogWarning("GetResourceAmount: Unknown resource type: " + resourceType);
                    return 0;
            }
        }

        private void TrySaveData(ResourceData data)
        {
            if (SaveLoadManager.Instance.TrySaveData_Local<ResourceData>(data,"ResourceData")) 
            {
                this.data = data;
                OnResourceDataChanged?.Invoke(this, data);
            }
            else Debug.LogWarning("Save resource data fail");
        }
        
        public void SetData(ResourceData data)
        {
            this.data = data;
        }
    }

    [Serializable]
    public class ResourceData
    {
        public int gold;
        public int diamond;
        public int stamina;

        public ResourceData(int gold = 0, int diamond = 0, int stamina = 0)
        {
            this.gold = gold;
            this.diamond = diamond;
            this.stamina = stamina;
        }

        // parameterless constructor for generic types
        public ResourceData()
        {
            gold = 0;
            diamond = 0;
            stamina = 0;
        }

        // Copy constructor
        public ResourceData(ResourceData other)
        {
            this.gold = other.gold;
            this.diamond = other.diamond;
            this.stamina = other.stamina;
        }
    }

    public enum ResourceType
    {
        Gold, Diamond, Stamina
    }

}
