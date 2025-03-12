using System;
using Flappy.Core.Manager;
using UnityEngine;

namespace Flappy.Script.SO
{
    [CreateAssetMenu(fileName = "DailyRewardSO", menuName = "NewSO/Rewards/_DailyRewardSO", order = 1)]
    [Serializable]
    public class DailyRewardSO : ScriptableObject
    {
        public Reward[] dailyRewards = new Reward[7];

        public void ReSetData()
        {
            foreach (var reward in dailyRewards)
            {
                reward.isTodayReward = false;
                reward.isClaimed = false;
            }
        }
    }

}
