using System;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class RewardUIItem : MonoBehaviour
    {
        // public Image rewardIcon;   // Icon của phần thưởng (vàng, kim cương,...)
        public TMP_Text valueText;    // Text hiển thị số lượng hoặc thông tin phần thưởng
        public Button claimButton; // Nút để nhận phần thưởng
        public Reward reward;      // Instance của Reward

        [SerializeField] GameObject focus;
        [SerializeField] GameObject clear;

        // cần bỏ đoạn code này, phải kéo thả trực tiếp, vì khi bắt đầu thì các RewardUIItem đều bị inactive thoe parent là container của DailyRewardPanel
        // dẫn đến việc các hàm Find, GetComponent không hoạt động được
        // void Awake() 
        // {
        //     if (valueText == null) valueText = transform.Find("Text_Value").GetComponent<TextMeshProUGUI>();
        //     if (focus == null) focus = transform.Find("Focus").gameObject;
        //     if (clear == null) clear = transform.Find("Clear").gameObject;
        //     if (claimButton == null) claimButton = GetComponent<Button>();
        // }

        void Start()
        {
            UpdateUI(); // Cập nhật UI khi khởi tạo
            claimButton.onClick.AddListener(CollectReward); // Gắn sự kiện cho nút
        }

        public void UpdateUI()
        {
            if (reward != null)
            {
                valueText.text = $"{reward.colectableSO.type} x{reward.colectableSO.amount}";
                // Cập nhật icon dựa trên rewardType nếu cần (có thể thêm logic sau)
                // claimButton.interactable = !reward.isClaimed; // Vô hiệu hóa nút nếu đã nhận
                focus.SetActive(false);
                clear.SetActive(false);

                if (reward.isTodayReward && !reward.isClaimed)
                {
                    focus.SetActive(true);
                }
                else if (reward.isClaimed)
                {
                    clear.SetActive(true);
                }

            }
        }

        public void CollectReward()
        {
            if (reward != null)
            {
                DailyRewardManager.Instance.ClaimReward(reward.dayInWeak);
                claimButton.interactable = false;
            }
        }
    }
}
