using System;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    // public class ResourceManager : PersistentSingleton<ResourceManager>
    // {
    //     public ResourceData resourceData;

    //     void Start()
    //     {
    //         SaveLoadManager.Instance.OnGameDataChanged += LoadResourceData;
    //         resourceData = SaveLoadManager.Instance.gameData.resourceData;
    //     }

    //     private void LoadResourceData(object sender, GameData e)
    //     {
    //         resourceData = e.resourceData;
    //     }

    //     public void AddResource(ResourceType resourceType, int amount)
    //     {
    //         switch (resourceType)
    //         {
    //             case ResourceType.Gold:
    //                 resourceData.gold += amount;
    //                 break;
    //             case ResourceType.Diamond:
    //                 resourceData.diamond += amount;
    //                 break;
    //             case ResourceType.Stamina:
    //                 resourceData.stamina += amount;
    //                 break;
    //             default:
    //                 Debug.LogWarning("AddResource: Unknown resource type: " + resourceType);
    //                 break;
    //         }

    //         // OnResourceDataChanged?.Invoke(this, resourceData);
    //         SaveLoadManager.Instance.gameData.resourceData = resourceData;
    //         SaveLoadManager.Instance.CallInvoke();
    //     }

    //     public void SpendResource(ResourceType resourceType, int amount)
    //     {
    //         switch (resourceType)
    //         {
    //             case ResourceType.Gold:
    //                 resourceData.gold = resourceData.gold >= amount ? resourceData.gold - amount : resourceData.gold;
    //                 break;
    //             case ResourceType.Diamond:
    //                 resourceData.diamond = resourceData.diamond >= amount ? resourceData.diamond - amount : resourceData.diamond;
    //                 break;
    //             case ResourceType.Stamina:
    //                 resourceData.stamina = resourceData.stamina >= amount ? resourceData.stamina - amount : resourceData.stamina;
    //                 break;
    //             default:
    //                 Debug.LogWarning("SpendResource: Unknown resource type: " + resourceType);
    //                 break;
    //         }

    //         // OnResourceDataChanged?.Invoke(this, resourceData);
    //         SaveLoadManager.Instance.gameData.resourceData = resourceData;
    //         SaveLoadManager.Instance.CallInvoke();
    //     }

    //     public int GetResourceAmount(ResourceType resourceType)
    //     {
    //         switch (resourceType)
    //         {
    //             case ResourceType.Gold:
    //                 return resourceData.gold;
    //             case ResourceType.Diamond:
    //                 return resourceData.diamond;
    //             case ResourceType.Stamina:
    //                 return resourceData.stamina;
    //             default:
    //                 Debug.LogWarning("GetResourceAmount: Unknown resource type: " + resourceType);
    //                 return 0;
    //         }
    //     }
    // }

    public class ResourceManager : PersistentSingleton<ResourceManager>
    {
        public ResourceData data;

        private ResourceData tempData;  // when save load error occurs, this will make sure the real data is not be effected

        void Start()
        {
            SaveLoadManager.Instance.OnResourceDataChanged += UpdateResourceData;
        }

        private void UpdateResourceData(object sender, ResourceData e)
        {
            data = e;
        }

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

            SaveLoadManager.Instance.TrySaveResourceData(tempData);
        }

        public void SpendResource(ResourceType resourceType, int amount)
        {
            tempData = new ResourceData(data);
            
            switch (resourceType)
            {
                case ResourceType.Gold:
                    if(tempData.gold < amount) return;
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

            SaveLoadManager.Instance.TrySaveResourceData(tempData);
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
