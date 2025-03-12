using System;
using UnityEngine;
using AjaxNguyen.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Flappy.Script.SO;

namespace Flappy.Core.Manager
{
    public class DailyRewardManager : PersistentSingleton<DailyRewardManager>
    {
        public event EventHandler<DailyRewardData> OnDailyRewardDataChanged;
        public DailyRewardSO SOData; // Tham chiếu đến dữ liệu phần thưởng
        [SerializeField] private DateTime startDate = new(2025, 3, 3); // Ngày bắt đầu chu kỳ (CN tuần trước)
        private int currentDay = 0;          // Ngày hiện tại (0-6)

        [SerializeField] private DailyRewardData data;
        private DailyRewardData tempData;

        public const string FILE_NAME_DAILY_REWARD = "DailyRewardData";

        protected override void Awake()
        {
            base.Awake();
            data = new();
        }

        // void Start()
        // {
        //     // TrySaveData(data); // DO NOT DELETE: đoạn này để đẩy dữ liệu thủ công vào json.
        //     FirstLoad();
        // }

        public void SetData(DailyRewardData inputData)
        {
            this.data = inputData;
            foreach (var kvp in inputData.statusDic)
            {
                int dayIndex = kvp.Key;

                if (dayIndex >= 0 && dayIndex < SOData.dailyRewards.Length)
                {
                    SOData.dailyRewards[dayIndex].isClaimed = kvp.Value;
                }
                else
                {
                    Debug.LogWarning($"Day index {dayIndex} nằm ngoài phạm vi hợp lệ!");
                }
            }

            OnDailyRewardDataChanged?.Invoke(this, data);
        }

        private async Task TrySaveData(DailyRewardData inputData)
        {
            tempData = inputData;

            if (await SaveLoadManager.Instance.TrysaveData_Both(tempData, "DailyRewardData"))
            {
                this.data = inputData;
                OnDailyRewardDataChanged?.Invoke(this, inputData);
            }
            else Debug.LogWarning("Save dailyReward data fail");
        }

        public async void ClaimReward(int day) // from 0 to 6
        {
            if (!IsNetworkAvailable())
            {
                PanelManager.Instance.ShowErrorPopup("Network is not available");
                return;
            }

            if (SOData.dailyRewards[day].isClaimed)
            {
                Debug.Log("Không thể nhận thưởng: Phần thưởng này đã nhận!");
                return;
            }

            if (!SOData.dailyRewards[day].isTodayReward)
            {
                Debug.Log("Không thể nhận thưởng: Đây không phải phần thưởng hôm nay!");
                return;
            }

            tempData = new(data);
            tempData.statusDic[day] = true;

            try
            {
                currentDay = await GetCurrentDayFromServer();

                if (currentDay != day) return;

                bool result = await SaveLoadManager.Instance.TrysaveData_Both(tempData, SaveLoadManager.FILE_NAME_DAILY_REWARD);

                if (result)
                {
                    data = new(tempData);
                    SOData.dailyRewards[day].isClaimed = true;
                    SOData.dailyRewards[day].colectableSO.Collect();
                    OnDailyRewardDataChanged?.Invoke(this, data);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        // Kiểm tra ngày hiện tại trong tuần dựa trên thời gian server
        private async Task<int> GetCurrentDayFromServer()
        {
            DateTime serverTime = await ServerTimeManager.Instance.GetServerTimeAsync();
            return GetDayFromTime(serverTime);
        }

        private int GetWeekFromTime(DateTime time)
        {
            int daysSinceStart = (time - startDate).Days;
            return Mathf.FloorToInt(daysSinceStart / 7);
        }

        private int GetDayFromTime(DateTime time)
        {
            int daysSinceStart = (time - startDate).Days;
            return daysSinceStart % 7; // 0 đén 6
        }

        private bool IsNetworkAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public async void FirstLoad()
        {
            if (!IsNetworkAvailable())
            {
                PanelManager.Instance.ShowErrorPopup("Network is not available");
                return;
            }

            DateTime serverTime = await ServerTimeManager.Instance.GetServerTimeAsync();
            DateTime lastLogInTime = await AuthManager.Instance.GetLastLoginTimeFromCloudAsync();
            await LoadData_Cloud();

            var currentDayIndex = GetDayFromTime(serverTime);
            var currentWeekIndex = GetWeekFromTime(serverTime);
            var lastWeekIndex = GetWeekFromTime(lastLogInTime);

            Debug.Log("Current week index: " + currentWeekIndex);
            Debug.Log("Last week index: " + lastWeekIndex);

            if (lastWeekIndex != currentWeekIndex) // đã sang tuần mới, reset
            {
                data = new();
                SOData.ReSetData();
            }

            if (SOData.dailyRewards[currentDayIndex].isTodayReward == false)
            {
                SOData.dailyRewards[currentDayIndex].isTodayReward = true;
            }

            OnDailyRewardDataChanged?.Invoke(this, data);
        }

        async Task LoadData_Cloud()
        {
            try
            {
                var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { FILE_NAME_DAILY_REWARD });

                if (loadData.TryGetValue(FILE_NAME_DAILY_REWARD, out var dailyRewardItem))
                {
                    data = dailyRewardItem.Value.GetAs<DailyRewardData>();
                    SetData(data);
                }
                else
                {
                    Debug.LogError($"Data with key '{FILE_NAME_DAILY_REWARD}' not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading data from Cloud Save: {ex.Message}");
            }
        }

    }


    [Serializable]
    public class DailyRewardData
    {
        public Dictionary<int, bool> statusDic = new(); // Trạng thái của từng phần thưởng (dayIndex -> isClaimed)

        public DailyRewardData()
        {
            for (int i = 0; i < 7; i++) statusDic.Add(i, false);
        }

        //copy constructor
        public DailyRewardData(DailyRewardData other)
        {
            statusDic = new Dictionary<int, bool>(other.statusDic);
        }
    }


    public enum RewardType { Gold, Diamond, Stamina, LuckyEgg }

    public abstract class CollectibleSO : ScriptableObject
    {
        public RewardType type;
        public int amount;

        public abstract void Collect();
    }

    // [CreateAssetMenu(fileName = "ResourceRewardSO", menuName = "NewSO/Rewards/ResourceReward")]
    // public class ResourceRewardSO : CollectibleSO
    // {
    //     public override void Collect()
    //     {
    //         ResourceManager.Instance.AddResource(type, amount);
    //         Debug.Log($"Collected Resource: {type} x{amount}");
    //     }
    // }

    // [CreateAssetMenu(fileName = "StaminaRewardSO", menuName = "NewSO/Rewards/StaminaReward")]
    // public class StaminaRewardSO : CollectibleSO
    // {
    //     public override void Collect()
    //     {
    //         StaminaManager.Instance.AddStamina(amount);
    //         Debug.Log($"Collected Stamina: {amount}");
    //     }
    // }

    [Serializable]
    public class Reward
    {
        public CollectibleSO colectableSO;
        public int dayInWeak = 0;
        public bool isClaimed;        // Trạng thái đã nhận hay chưa
        public bool isTodayReward;

        public Reward(CollectibleSO colectableSO, bool isClaimed = false, bool isTodayReward = false)
        {
            this.colectableSO = colectableSO;
            this.isClaimed = isClaimed;
            this.isTodayReward = isTodayReward;
        }
    }

    // [CreateAssetMenu(fileName = "DailyRewardSO", menuName = "NewSO/Rewards/_DailyRewardSO", order = 1)]
    // [Serializable]
    // public class DailyRewardSO : ScriptableObject
    // {
    //     public Reward[] dailyRewards = new Reward[7];

    //     public void ReSetData()
    //     {
    //         foreach (var reward in dailyRewards)
    //         {
    //             reward.isTodayReward = false;
    //             reward.isClaimed = false;
    //         }
    //     }
    // }

}
