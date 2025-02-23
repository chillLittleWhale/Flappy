using AjaxNguyen.Core.Manager;
using AjaxNguyen.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen.Core.Panels
{
    public class SettingPanel : Panel
    {
        [SerializeField] Button logoutButton;

        public override void Initialize()
        {
            base.Initialize();
            logoutButton.onClick.AddListener(SignOut);
        }

        private async void SignOut()
        {
            await AuthManager.Instance.SignOut();
        }
    }
}
