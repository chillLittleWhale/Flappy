using System;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class ResourceManager : PersistentSingleton<ResourceManager>
    {
        public ResourceData resourceData;

        void Start()
        {
            SaveLoadManager.Instance.OnGameDataChanged += LoadResourceData;
            resourceData = SaveLoadManager.Instance.gameData.resourceData;
        }

        private void LoadResourceData(object sender, GameData e)
        {
            resourceData = e.resourceData;
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Gold:
                    resourceData.gold += amount;
                    break;
                case ResourceType.Diamond:
                    resourceData.diamond += amount;
                    break;
                case ResourceType.Stamina:
                    resourceData.stamina += amount;
                    break;
                default:
                    Debug.LogWarning("AddResource: Unknown resource type: " + resourceType);
                    break;
            }

            // OnResourceDataChanged?.Invoke(this, resourceData);
            SaveLoadManager.Instance.gameData.resourceData = resourceData;
            SaveLoadManager.Instance.CallInvoke();
        }

        public void SpendResource(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Gold:
                    resourceData.gold = resourceData.gold >= amount ? resourceData.gold - amount : resourceData.gold;
                    break;
                case ResourceType.Diamond:
                    resourceData.diamond = resourceData.diamond >= amount ? resourceData.diamond - amount : resourceData.diamond;
                    break;
                case ResourceType.Stamina:
                    resourceData.stamina = resourceData.stamina >= amount ? resourceData.stamina - amount : resourceData.stamina;
                    break;
                default:
                    Debug.LogWarning("SpendResource: Unknown resource type: " + resourceType);
                    break;
            }

            // OnResourceDataChanged?.Invoke(this, resourceData);
            SaveLoadManager.Instance.gameData.resourceData = resourceData;
            SaveLoadManager.Instance.CallInvoke();
        }

        public int GetResourceAmount(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Gold:
                    return resourceData.gold;
                case ResourceType.Diamond:
                    return resourceData.diamond;
                case ResourceType.Stamina:
                    return resourceData.stamina;
                default:
                    Debug.LogWarning("GetResourceAmount: Unknown resource type: " + resourceType);
                    return 0;
            }
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
    }

    public enum ResourceType
    {
        Gold, Diamond, Stamina
    }

}
