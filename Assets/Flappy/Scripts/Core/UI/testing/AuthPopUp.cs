using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy
{
    public class AuthPopUp : Panel
    {
        [SerializeField] TMP_InputField nameIpF;
        [SerializeField] TMP_InputField passwordIpF;
        [SerializeField] Button signInButton;
        [SerializeField] Button signUpButton;
        [SerializeField] Button signInAnonymousButton;


        private bool isSigningIn = false;
        private bool isSigningUp = false;

        public override void Initialize()
        {
            base.Initialize();
            signInButton.onClick.AddListener(SignIn);
            signUpButton.onClick.AddListener(SignUp);
            signInAnonymousButton.onClick.AddListener(SignInAnonymous);
        }

        public override void Open()
        {
            base.Open();
            nameIpF.text = "";
            passwordIpF.text = "";
        }

        private async void SignIn()
        {
            if (isSigningIn) return; // Ngăn chặn nhiều lần đăng nhập
            isSigningIn = true;

            string name = nameIpF.text.Trim();
            string password = passwordIpF.text.Trim();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
            {
                await AuthManager.Instance.SignInWithUsernameAndPasswordAsync(name, password);

                PanelManager.Instance.ClosePanel(PanelType.Authen);
            }

            isSigningIn = false;
        }


        private async void SignUp()
        {
            if (isSigningUp) return;
            isSigningUp = true;

            string name = nameIpF.text.Trim();
            string password = passwordIpF.text.Trim();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
            {
                if (IsPasswordValid(password))
                {
                    await AuthManager.Instance.SignUpWithUsernameAndPasswordAsync(name, password);
                }
                else
                {
                    PanelManager.Instance.ShowErrorPopup("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                }
            }

            isSigningUp = false;
        }

        private async void SignInAnonymous()
        {
            await AuthManager.Instance.SignInAnonymousAsync();
        }

        bool IsPasswordValid(string password)
        {
            if (password.Length < 8 || password.Length > 30) return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSymbol = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSymbol = true;
            }

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }
    }
}
