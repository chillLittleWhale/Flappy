using UnityEngine;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using System;
using AjaxNguyen.Event;
using AjaxNguyen.Core.UI;
using AjaxNguyen.Utility.Service;

namespace Flappy.Core.Manager
{
    public class AuthManager : PersistentSingleton<AuthManager>
    {
        [SerializeField] BoolEventChanel onLogin; //true = online, false = offline
        [SerializeField] EmptyEventChanel onSignout;

        private bool eventInitiated = false;
        private bool isSigningIn = false;
        private bool isSigningUp = false;

        JsonDataService dataService;

        async Task Start()
        {
            dataService = new();
            await StartGame();
        }

        private async Task StartGame()
        {
            if (IsNetworkAvailable())
            {
                await StartGameOnlineMode();
            }
            else
            {
                StartGameOfflineMode();
            }
        }

        private void StartGameOfflineMode()
        {
            string savedAccountId = PlayerPrefs.GetString("CurrentAccountID", string.Empty);

            if (!string.IsNullOrEmpty(savedAccountId)) // Người dùng đã đăng nhập trước đó, tải dữ liệu cục bộ
            {
                Debug.Log("No network connection. Starting game in offline mode with saved playerId: " + savedAccountId + ".");
                onLogin.Raise(false); // Saveload sẽ nhận event này và quyết định cách tải dữ liệu
            }
            else
            {
                // Người dùng chưa đăng nhập trước đó
                Debug.LogWarning("No internet connection and no saved account. Cannot start game.");
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.StartSevice, "No account loged in and no internet connection, try latter ");
            }
        }

        private async Task StartGameOnlineMode()
        {
            await InitializeUGS();
            await AutoLogIn();  //TODO: chua chac da log in thanh cong o day
        }

        public async Task InitializeUGS()
        {
            try
            {
                if (UnityServices.State != ServicesInitializationState.Uninitialized)
                {
                    Debug.Log("UGS is already initialized or initializing.");
                    return;
                }
                else //if (UnityServices.State == ServicesInitializationState.Uninitialized)
                {
                    var option = new InitializationOptions();
                    option.SetProfile("default_profile");
                    await UnityServices.InitializeAsync(option);
                }

                if (!eventInitiated)
                {
                    SetupEvents();
                }

            }
            catch (Exception e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.StartSevice, "Failed to conect to network ");
                Debug.Log(e.Message);
            }

        }

        private async Task AutoLogIn()
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                PanelManager.Instance.OpenPanel(PanelType.MainMenu);
                onLogin.Raise(false);
            }
            else if (AuthenticationService.Instance.SessionTokenExists)
            {
                if (await SignInAnonymousAsync()) onLogin.Raise(false);
                else
                {
                    PanelManager.Instance.OpenPanel(PanelType.Authen);
                }

            }
            else
            {
                PanelManager.Instance.OpenPanel(PanelType.Authen);
                Debug.Log("auth panel");
            }
        }

        public async Task<bool> SignInAnonymousAsync()
        {
            Debug.Log("SignInAnonymousAsync");
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PanelManager.Instance.OpenPanel(PanelType.MainMenu);
                return true;
            }
            catch (AuthenticationException e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "SignInAnonymousAsync: Failed to sign in");
                Debug.Log(e.Message);
                return false;
            }
            catch (RequestFailedException e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "SignInAnonymousAsync: Failed to conect to network ");
                Debug.Log(e.Message);
                return false;
            }
            catch (Exception e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "SignInAnonymousAsync: unexpected error occurred ");
                Debug.LogError($"An unexpected error occurred: {e.Message}");
                return false;
            }

        }

        public async Task SignInWithUsernameAndPasswordAsync(string username, string password)
        {
            if (AuthenticationService.Instance.IsSignedIn || isSigningIn) return;

            isSigningIn = true; // Đặt cờ để ngăn các lần gọi tiếp theo

            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);

                while (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerId))
                {
                    await Task.Delay(50); // Chờ thông tin PlayerID được cập nhật
                }

                PlayerPrefs.SetString("CurrentAccountID", AuthenticationService.Instance.PlayerId);
                onLogin.Raise(true);

                PanelManager.Instance.OpenPanel(PanelType.MainMenu);
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"SignInWithUsernameAndPassword Failed: {e.Message}");
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"SignInWithUsernameAndPassword Failed: {e.Message}");
            }
            finally
            {
                isSigningIn = false;
            }
        }

        public async Task SignUpWithUsernameAndPasswordAsync(string username, string password)
        {
            if (isSigningUp) return;
            isSigningUp = true;

            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);

                while (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerId))
                {
                    await Task.Delay(50);
                }

                PlayerPrefs.SetString("CurrentAccountID", AuthenticationService.Instance.PlayerId);

                PanelManager.Instance.OpenPanel(PanelType.MainMenu);
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"SignUpWithUsernameAndPassword Failed: {e.Message}");
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"SignUpWithUsernameAndPassword Failed: {e.Message}");
            }
            finally { isSigningUp = false; }
        }

        public async Task SignOut()
        {
            PlayerPrefs.DeleteKey("CurrentAccountID");
            PlayerPrefs.Save();

            onSignout.Raise(new Empty());
            await Task.Delay(1000);  // đợi cho các listenner thực hiện hết

            AuthenticationService.Instance.SignOut(true);  // để true để nó xóa hết credential

            PanelManager.Instance.CloseAllPanels();
            PanelManager.Instance.OpenPanel(PanelType.Authen);
        }

        private void SetupEvents()
        {
            eventInitiated = true;

            AuthenticationService.Instance.SignedIn += async () =>
            {
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
                Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

                await SignInConfirmedAsync();
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("SetupEvents: Player signed out.");
                PanelManager.Instance.OpenPanel(PanelType.Authen);
            };

            AuthenticationService.Instance.Expired += async () =>
            {
                Debug.Log("SetupEvents: Player session could not be refreshed and expired.");
                await SignInAnonymousAsync();
            };
        }

        private async Task SignInConfirmedAsync()
        {
            Debug.Log("SignInConfirmedAsync");
            try
            {
                if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync("New_Player"); // hàm này yêu cầu input không được có space
                    // PanelManager.Instance.OpenPanel("main");
                }
            }
            catch (AuthenticationException ex)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.SignIn, "SignInConfirmedAsync:Failed to sign in");
                Debug.LogWarning(ex.Message);
            }
            catch (RequestFailedException)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.SignIn, "SignInConfirmedAsync: Failed to conect to network ");
            }
        }

        // Hàm kiểm tra kết nối mạng
        bool IsNetworkAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

    }

}