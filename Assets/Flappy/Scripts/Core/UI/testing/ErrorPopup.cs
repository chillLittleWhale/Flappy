
using System;
using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy
{
    public class ErrorPopup : Panel
    {
        public enum Action{
            None = 0, StartSevice = 1, SignIn =2, OpenAuthMenu =3
        }

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button button;
        private Action action = Action.None;

        public override void Initialize()
        {
            base.Initialize();
            button.onClick.AddListener(ButtonAction);
        }

        public void Open(Action action, string message)
        {
            base.Open();
            this.action = action;
            if (!string.IsNullOrEmpty(message))
            {
                titleText.text = message;
            }
        }

        private async void ButtonAction()
        {
            Close();

            switch (action)
            {
                case Action.StartSevice:
                    await AuthManager.Instance.InitializeUGS();
                    break;
                case Action.SignIn:
                    await AuthManager.Instance.SignInAnonymousAsync();
                    break;
                case Action.OpenAuthMenu:
                    PanelManager.Instance.OpenPanel(PanelType.Authen);
                    break;
                default:
                    break;
            }
        }


    }
}
