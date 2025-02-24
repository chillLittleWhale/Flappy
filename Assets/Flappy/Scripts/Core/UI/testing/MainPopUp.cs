using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy
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

        private async void SignOut()
        {
            await AuthManager.Instance.SignOut();
        }

        private void UpdateUI()
        {
            nameText.text = AuthenticationService.Instance.PlayerName;
        }
    }
}
