using UnityEngine;
using System;

namespace Flappy.Core
{
    [Serializable]
    public class Reward
    {
        public enum RewardType
        {
            Gold,Gem,LuckyEgg 
        }
        // [Serializable]
        // public enum RewardStatus
        // {
        //     NotClaimable, Claimed, Claimable
        // }

        public int dayInWeak = 0;
        public RewardType rewardType; // Loại phần thưởng
        public int amount;            // Số lượng
        public bool isClaimed;        // Trạng thái đã nhận hay chưa
        public bool isTodayReward; 
        // public RewardStatus rewardStatus;

        // public Reward(RewardType type, int amount, bool isClaimed = false, RewardStatus rewardStatus = RewardStatus.NotClaimable)
        // {
        //     this.rewardType = type;
        //     this.amount = amount;
        //     this.isClaimed = isClaimed; 
        //     this.rewardStatus = rewardStatus;
        // }


        public Reward(RewardType type, int amount, bool isClaimed = false, bool isTodayReward = false)
        {
            this.rewardType = type;
            this.amount = amount;
            this.isClaimed = isClaimed; 
            this.isTodayReward = isTodayReward;
        }

        // public void Collect()  //TODO
        // {
        //     if (!isClaimed)
        //     {
        //         // Logic thu thập phần thưởng (ví dụ: thêm vào kho người chơi)
        //         isClaimed = true;
        //         Debug.Log($"Đã nhận {amount} {rewardType}");
        //     }
        //     else
        //     {
        //         Debug.Log("Phần thưởng đã được nhận trước đó!");
        //     }
        // }
    }
}
