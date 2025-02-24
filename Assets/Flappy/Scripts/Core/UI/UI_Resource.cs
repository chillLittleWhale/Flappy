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
            // SaveLoadManager.Instance.OnResourceDataChanged += UpdateUI; //--
            // SaveLoadManager.Instance.CallInvoke(); //--

            StartCoroutine(DelayRegisterEvent());
            ResourceManager.Instance.OnResourceDataChanged += UpdateUI;
            
        }

        private void UpdateUI(object sender, ResourceData e)
        {
            diamonText.text = e.diamond.ToString();
            goldText.text = e.gold.ToString();
            staminaText.text = e.stamina.ToString();
        }

        private IEnumerator DelayRegisterEvent()
        {
            // chờ 1 frame để ResourceManager tạo xong dữ liệu
            yield return null;
            UpdateUI(this, ResourceManager.Instance.data);
        }
    }

}
