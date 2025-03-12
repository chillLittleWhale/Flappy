using System.Collections.Generic;
using System.Linq;
using AjaxNguyen.Core.UI;
using Flappy.Core;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.Panels
{
    public class GachaPanel : Panel
    {
        [SerializeField] TextMeshProUGUI textName;
        [SerializeField] int nomalGachaCost = 1000; // gold
        [SerializeField] int advancedGachaCost = 50; // diamon

        [SerializeField] Button nomalGachaButton;
        [SerializeField] Button advancedGachaButton;
        public Image image;

        private Skin reward;
        [SerializeField] SkinData skinData;

        void Start()
        {
            skinData = SkinManager.Instance.data; // data binding 

            nomalGachaButton.onClick.AddListener(OnClickNomalGacha);
            advancedGachaButton.onClick.AddListener(OnClickAdvancedGacha);
        }

        public void OnClickNomalGacha() // gacha bằng vàng, có thể ra skin đã có
        {
            if (ResourceManager.Instance.TrySpendResource(RewardType.Gold, nomalGachaCost))
            {
                reward = Gacha(skinData.skinList);
                UpdateUI();
                ClaimReward();
            }
            else
            {
                PanelManager.Instance.ShowErrorPopup("Không đủ vàng để gacha.");
            }
        }

        public void OnClickAdvancedGacha()  // gacha bằng đá quý, ưu tiên skin chưa có
        {
            if (ResourceManager.Instance.TrySpendResource(RewardType.Diamond, advancedGachaCost))
            {
                List<Skin> a = GetLockedSkins();
                if (a.Count == 0) a = skinData.skinList;
                reward = Gacha(a);
                UpdateUI();
                ClaimReward();
            }
            else
            {
                PanelManager.Instance.ShowErrorPopup("Không đủ kim cương để gacha.");
            }
        }

        private void ClaimReward()
        {
            if (reward == null)
            {
                PanelManager.Instance.ShowErrorPopup("reward == null");
                return;
            }
            if (reward.isUnlocked == true) // already have this skin, convert to gold reward
            {
                ResourceManager.Instance.AddResource(RewardType.Gold, reward.unlockCost);
                PanelManager.Instance.ShowErrorPopup($"Already have this skin, convert to gold x{reward.unlockCost}.");
            }
            else
            {
                SkinManager.Instance.UnlockSkin(reward.id);
                Debug.Log($"Success unlock skin {reward.skinName} (id: {reward.id}).");
            }
        }

        private Skin Gacha(List<Skin> skinList)
        {
            if (skinList != null)
            {
                int randomIndex = Random.Range(0, skinList.Count);
                return skinList[randomIndex];
            }
            return null;
        }

        private void UpdateUI()
        {
            textName.text = reward.skinName;
            image.sprite = reward.skinIcon;
        }

        private List<Skin> GetLockedSkins()
        {
            return skinData.skinList.Where(skin => !skin.isUnlocked).ToList();
        }
    }
}
