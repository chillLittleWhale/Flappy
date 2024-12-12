using System;
using AjaxNguyen.Core.Manager;
using TMPro;
using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    // public class UI_Resource : MonoBehaviour
    // {
    //     [SerializeField] private TextMeshProUGUI diamonText;
    //     [SerializeField] private TextMeshProUGUI goldText;
    //     [SerializeField] private TextMeshProUGUI staminaText;


    //     void Start()
    //     {
    //         SaveLoadManager.Instance.OnGameDataChanged += UpdateUI;
    //         SaveLoadManager.Instance.CallInvoke();
    //     }

    //     private void UpdateUI(object sender, GameData e)
    //     {
    //         diamonText.text = e.resourceData.diamond.ToString();
    //         goldText.text = e.resourceData.gold.ToString();
    //         staminaText.text = e.resourceData.stamina.ToString();
    //     }
    // }
        public class UI_Resource : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamonText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI staminaText;


        void Start()
        {
            SaveLoadManager.Instance.OnResourceDataChanged += UpdateUI;
            SaveLoadManager.Instance.CallInvoke();
        }

        private void UpdateUI(object sender, ResourceData e)
        {
            diamonText.text = e.diamond.ToString();
            goldText.text = e.gold.ToString();
            staminaText.text = e.stamina.ToString();
        }
    }

}
