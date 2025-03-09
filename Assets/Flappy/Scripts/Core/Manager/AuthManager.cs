using UnityEngine;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using System;
using AjaxNguyen.Event;
using AjaxNguyen.Core.UI;
using AjaxNguyen.Utility.Service;
using AjaxNguyen.Utility.Event;
using System.Collections.Generic;
using Unity.Services.CloudSave;

namespace Flappy.Core.Manager
{
    public class AuthManager : PersistentSingleton<AuthManager>
    {
        private const string LAST_LOGIN_KEY = "LastLoginTime";
        [SerializeField] BoolEventChanel onLogin; //true = online, false = offline
        [SerializeField] EmptyEventChanel onSignoutAttempt; // 


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
                // EventSystem.Instance.RaiseEventAsync("SignInEvent", false);
            }
            else
            {
                // Người dùng chưa đăng nhập trước đó
                Debug.LogWarning("No internet connection and no saved account. Cannot start game.");
                PanelManager.Instance.ShowErrorPopup("No account loged in and no internet connection, try latter", ErrorPopup.Action.StartSevice);
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
                PanelManager.Instance.ShowErrorPopup("Failed to conect to network ", ErrorPopup.Action.StartSevice);
                Debug.Log(e.Message);
            }

        }

        private async Task AutoLogIn()
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                PanelManager.Instance.OpenPanel(PanelType.MainMenu);
                onLogin.Raise(false);
                // await EventSystem.Instance.RaiseEventAsync("SignInEvent", false);
            }
            else if (AuthenticationService.Instance.SessionTokenExists)
            {
                if (await SignInAnonymousAsync())
                {
                    onLogin.Raise(false);
                    // await EventSystem.Instance.RaiseEventAsync("SignInEvent", false);
                }
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
                PanelManager.Instance.ShowErrorPopup("SignInAnonymousAsync: Failed to sign in");
                Debug.Log(e.Message);
                return false;
            }
            catch (RequestFailedException e)
            {
                PanelManager.Instance.ShowErrorPopup("SignInAnonymousAsync: Failed to conect to network ");
                Debug.Log(e.Message);
                return false;
            }
            catch (Exception e)
            {
                PanelManager.Instance.ShowErrorPopup("SignInAnonymousAsync: unexpected error occurred ");
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
                // await EventSystem.Instance.RaiseEventAsync("SignInEvent", true);

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
                await EventSystem.Instance.RaiseEventAsync("SignUpEvent");
                onLogin.Raise(false);

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

        // public async Task SignOut()
        // {
        //     Debug.Log("AuthManager: SignOut");
        //     PlayerPrefs.DeleteKey("CurrentAccountID");
        //     PlayerPrefs.Save();

        //     onSignoutAttempt.Raise(new Empty());
        //     await Task.Delay(2000);  // đợi cho các listenner thực hiện hết //TODO: xem xét phần này

        //     AuthenticationService.Instance.SignOut(true);  // để true để nó xóa hết credential

        //     PanelManager.Instance.CloseAllPanels();
        //     PanelManager.Instance.OpenPanel(PanelType.Authen);
        // }

        public async Task SignOut()
        {
            Debug.Log("AuthManager: SignOut");
            PlayerPrefs.DeleteKey("CurrentAccountID");
            PlayerPrefs.Save();

            await EventSystem.Instance.RaiseEventAsync("SignOutEvent", () =>
            {
                AuthenticationService.Instance.SignOut(true);  // để true để nó xóa hết credential
                PanelManager.Instance.CloseAllPanels();
                PanelManager.Instance.OpenPanel(PanelType.Authen);
            });
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

                var tmp = await ServerTimeManager.Instance.GetServerTimeAsync();
                await Task.Delay(5000);  // đợi cho DailyReawardManager thực hiện hết
                SaveLastLoginTimeToCloudAsync(tmp);
                
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
                await Task.Delay(1000); // Chờ 0.5 giây để dữ liệu đồng bộ từ server, nếu không tên sẽ null

                if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync("New_Player"); // hàm này yêu cầu input không được có space
                    // PanelManager.Instance.OpenPanel("main");
                }
            }
            catch (AuthenticationException ex)
            {
                PanelManager.Instance.ShowErrorPopup("SignInConfirmedAsync:Failed to sign in", ErrorPopup.Action.SignIn);
                Debug.LogWarning(ex.Message);
            }
            catch (RequestFailedException)
            {
                PanelManager.Instance.ShowErrorPopup("SignInConfirmedAsync: Failed to conect to network ", ErrorPopup.Action.SignIn);
            }
        }


        public async Task<DateTime> GetLastLoginTimeFromCloudAsync()
        {
            try
            {
                var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { LAST_LOGIN_KEY });
                if (data.TryGetValue(LAST_LOGIN_KEY, out var timeItem))
                {
                    return timeItem.Value.GetAs<DateTime>();
                }
                return DateTime.MinValue; // Không tìm thấy, trả về giá trị mặc định
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load last login time from Cloud: {e.Message}");
                return DateTime.MinValue;
            }
        }

        public async void SaveLastLoginTimeToCloudAsync(DateTime loginTime)
        {
            try
            {
                var data = new Dictionary<string, object>
            {
                { LAST_LOGIN_KEY, loginTime}//.ToFileTimeUtc() }
            };
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save last login time to Cloud: {e.Message}");
            }
        }

        bool IsNetworkAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

    }

}