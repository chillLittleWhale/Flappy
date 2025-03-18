using System.Threading.Tasks;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;

namespace Flappy.Core.UI
{
    public class UI_Resource : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamonText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI staminaText;


        void Start()
        {
            DelayUIUpdate();
            ResourceManager.Instance.OnResourceDataChanged += UpdateResourceUI;
            StaminaManager.Instance.OnStaminaChanged += UpdateStaminaUI;
        }

        void OnDestroy()
        {
            ResourceManager.Instance.OnResourceDataChanged -= UpdateResourceUI;
            StaminaManager.Instance.OnStaminaChanged -= UpdateStaminaUI;
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

        private async void DelayUIUpdate()
        {
            await Task.Delay(100);
            UpdateResourceUI(this, ResourceManager.Instance.data);
            UpdateStaminaUI(this, StaminaManager.Instance.GetStaminaStatus());
        }
    }

}
