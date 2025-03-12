using AjaxNguyen;
using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using Flappy.Core.UI;
using Flappy.Script.SO;

namespace Flappy.Core.Panels
{
    public class DailyRewardPanel : Panel
    {
        public RewardUIItem[] rewardUIItems; // Mảng các UI item cho từng ngày
        public DailyRewardSO dataSO;  // Tham chiếu đến dữ liệu DailyReward


        private DailyRewardData data;

        void Start()
        {
            dataSO = DailyRewardManager.Instance.SOData;
            DailyRewardManager.Instance.OnDailyRewardDataChanged += Reload;
            InitializeRewards(); // Khởi tạo UI khi bắt đầu
        }

        void InitializeRewards()
        {
            if (dataSO != null)// && rewardUIItems.Length == dataSO.dailyRewards.Length)
            {
                for (int i = 0; i < dataSO.dailyRewards.Length; i++)
                {
                    rewardUIItems[i].reward = dataSO.dailyRewards[i];
                    rewardUIItems[i].UpdateUI(); // Cập nhật UI cho từng ngày
                }
            }
        }

        private void Reload(object sender, DailyRewardData e)
        {
            InitializeRewards();
        }
    }
}
