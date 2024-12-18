using System.Collections.Generic;
using AjaxNguyen.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen
{
    public class TestSkingacha : MonoBehaviour
    {
        public TextMeshProUGUI textName;
        public TextMeshProUGUI textStatus;
        public Image image;

        public Skin reward;

        public List<Skin> skinList;


        public void OnButtonClick()
        {
            Gacha();
            UpdateUI();
        }

        void Gacha()
        {
            if (skinList != null)
            {
                int randomIndex = Random.Range(0, skinList.Count);
                reward = skinList[randomIndex];
            }
        }

        public void UpdateUI()
        {
            // textName.text = reward.skinData.name;
            textName.text = reward.skinName;

            textStatus.text = reward.isUnlocked ? "Unlocked" : "Locked";
            // image.sprite = reward.skinData.skinIcon;
            image.sprite = reward.skinIcon;
        }

    }
}
