using Flappy.Core.Manager;
using UnityEngine;

namespace Flappy.Script.SO
{
    [CreateAssetMenu(fileName = "ResourceRewardSO", menuName = "NewSO/Rewards/ResourceReward")]
    public class ResourceRewardSO : CollectibleSO
    {
        public override void Collect()
        {
            ResourceManager.Instance.AddResource(type, amount);
            Debug.Log($"Collected Resource: {type} x{amount}");
        }
    }
}
