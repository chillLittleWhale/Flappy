using UnityEngine;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using AjaxNguyen.Core.Service;
using AjaxNguyen.Event;

namespace AjaxNguyen.Core.Manager
{
    public class AuthManager : PersistentSingleton<AuthManager>
    {
        [SerializeField] BoolEventChanel onLoginOnline;
        private bool eventInitiated = false;
        private bool isSigningIn = false;

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
                onLoginOnline.Raise(false); // Saveload sẽ nhận event này và quyết định cách tải dữ liệu
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

        // public async Task InitializeUGS()
        // {
        //     try
        //     {
        //         if (UnityServices.State != ServicesInitializationState.Uninitialized)
        //         {
        //             Debug.Log("UGS is already initialized or initializing.");
        //             return;
        //         }
        //         else //if (UnityServices.State == ServicesInitializationState.Uninitialized)
        //         {
        //             var option = new InitializationOptions();
        //             option.SetProfile("default_profile");
        //             await UnityServices.InitializeAsync(option);
        //         }

        //         if (!eventInitiated)
        //         {
        //             SetupEvents();
        //         }

        //         if (AuthenticationService.Instance.IsSignedIn)
        //         {
        //             PanelManager.Instance.OpenPanel("main");
        //             Debug.Log("main panel");
        //         }
        //         else if (AuthenticationService.Instance.SessionTokenExists)
        //         {
        //             await SignInAnonymousAsync();
        //         }
        //         else
        //         {
        //             PanelManager.Instance.OpenPanel("auth");
        //             Debug.Log("auth panel");
        //         }

        //     }
        //     catch (Exception e)
        //     {
        //         PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.StartSevice, "Failed to conect to network ");
        //         Debug.Log(e.Message);
        //     }

        // }

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
                PanelManager.Instance.OpenPanel("main");
                Debug.Log("main panel");
                onLoginOnline.Raise(false);
            }
            else if (AuthenticationService.Instance.SessionTokenExists)
            {
                // await SignInAnonymousAsync();
                if (await SignInAnonymousAsync()) onLoginOnline.Raise(false);
                else
                {
                    PanelManager.Instance.OpenPanel("auth");
                }

            }
            else
            {
                PanelManager.Instance.OpenPanel("auth");
                Debug.Log("auth panel");
            }
        }

        public async Task<bool> SignInAnonymousAsync()
        {
            Debug.Log("SignInAnonymousAsync");
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PanelManager.Instance.OpenPanel("main");
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
                await Task.Delay(100);
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
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                PanelManager.Instance.OpenPanel("main");
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"SignUpWithUsernameAndPassword Failed: {e.Message}");
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"SignUpWithUsernameAndPassword Failed: {e.Message}");
            }
        }

        public void SignOut()
        {
            AuthenticationService.Instance.SignOut(true);  // để true để nó xóa hết credential
            PanelManager.Instance.CloseAllPanels();
            PanelManager.Instance.OpenPanel("auth");
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
                PanelManager.Instance.OpenPanel("auth");
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