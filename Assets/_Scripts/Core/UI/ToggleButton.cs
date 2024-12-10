using UnityEngine;
using TMPro;
using System;


namespace AjaxNguyen.Core.UI
{
    public class ToggleButton : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;

        private bool isOn = false;
        private float fillAmount;
        private string text;


        public Color onColor = Color.white;
        public Color offColor = Color.black;
        public float fillSpeed = 5f;


        void Start()
        {
            fillAmount = isOn ? 1 : 0;
            text = isOn ? "ON" : "OFF";
        }


        public void Toggle()
        {
            isOn = !isOn;
        
            UpdateStateText();
            UpdateHandlePivot();
        }

        private void UpdateHandlePivot()
        {
            fillAmount = isOn ? 1 : 0;
        }

        private void UpdateStateText()
        {
            buttonText.text = isOn ? "ON" : "OFF";
        }
    }
}
