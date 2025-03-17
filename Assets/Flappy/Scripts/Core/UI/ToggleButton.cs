using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


namespace AjaxNguyen.Core.UI
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] string toggleName = "EnterYourToggleName";
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Image fill;

        private bool isOn = false;
        // private float fillAmount;
        // private string text;


        [SerializeField] Color onColor = Color.white;
        [SerializeField] Color offColor = Color.black;
        // public float fillSpeed = 5f;

        bool GetToggleState() => PlayerPrefs.GetInt(toggleName, 1) == 1 ? true : false;  // 1 = on, 0 = off


        void Start()
        {
            isOn = GetToggleState();
        }

        void UpdateUI()
        {
            fill.color = isOn ? onColor : offColor;
            buttonText.text = isOn ? "ON" : "OFF";
        }

        public void Toggle()
        {
            isOn = !isOn;
            PlayerPrefs.SetInt(toggleName, isOn ? 1 : 0);
            UpdateUI();
        }
        // void Start()
        // {
        //     fillAmount = isOn ? 1 : 0;
        //     text = isOn ? "ON" : "OFF";
        // }


        // public void Toggle()
        // {
        //     isOn = !isOn;

        //     UpdateStateText();
        //     UpdateHandlePivot();
        // }

        // private void UpdateHandlePivot()
        // {
        //     fillAmount = isOn ? 1 : 0;
        // }

        // private void UpdateStateText()
        // {
        //     buttonText.text = isOn ? "ON" : "OFF";
        // }
    }
}
