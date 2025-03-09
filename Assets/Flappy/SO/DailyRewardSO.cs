using Flappy.Core;
using UnityEngine;

namespace Flappy
{

    [CreateAssetMenu(fileName = "DailyReward", menuName = "NewSO/DailyReward", order = 1)]
    [System.Serializable]
    public class DailyRewardSO : ScriptableObject
    {
        public Reward[] dailyRewards = new Reward[7]; // Mảng 7 phần thưởng cho 7 ngày

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
