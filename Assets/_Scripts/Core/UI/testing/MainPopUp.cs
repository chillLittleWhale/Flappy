using System;
using AjaxNguyen.Core.Manager;
using AjaxNguyen.Core.UI;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
namespace AjaxNguyen
{
    public class MainPopUp : Panel
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] Button logoutButton;

        public override void Initialize()
        {
            base.Initialize();
            logoutButton.onClick.AddListener(SignOut);
        }

        public override void Open()
        {
            base.Open();
            UpdateUI();
        }

        private void SignOut()
        {
            AuthManager.Instance.SignOut();
            PlayerPrefs.DeleteKey("CurrentAccountID");
            PlayerPrefs.Save();
        }

        private void UpdateUI()
        {
            nameText.text = AuthenticationService.Instance.PlayerName;
        }
    }
}
