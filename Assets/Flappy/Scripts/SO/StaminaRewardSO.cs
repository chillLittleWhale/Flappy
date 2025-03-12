using Flappy.Core.Manager;
using UnityEngine;

namespace Flappy.Script.SO
{
    [CreateAssetMenu(fileName = "StaminaRewardSO", menuName = "NewSO/Rewards/StaminaReward")]
    public class StaminaRewardSO : CollectibleSO
    {
        public override void Collect()
        {
            StaminaManager.Instance.AddStamina(amount);
            Debug.Log($"Collected Stamina: {amount}");
        }
    }

}
