using System.Collections;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class UI_Resource : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamonText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI staminaText;


        void Start()
        {
            StartCoroutine(DelayRegisterEvent());
            ResourceManager.Instance.OnResourceDataChanged += UpdateResourceUI;
            StaminaManager.Instance.OnStaminaChanged += UpdateStaminaUI;
        }

        private void UpdateResourceUI(object sender, ResourceData e)
        {
            diamonText.text = e.diamond.ToString();
            goldText.text = e.gold.ToString();
        }

        private void UpdateStaminaUI(object sender, string stamina)
        {
            staminaText.text = stamina;
        }

        private IEnumerator DelayRegisterEvent()
        {
            // chờ 1 frame để ResourceManager tạo xong dữ liệu
            yield return null;
            UpdateResourceUI(this, ResourceManager.Instance.data);
            UpdateStaminaUI(this, StaminaManager.Instance.GetStaminaStatus());
        }
    }

}
