using UnityEngine;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using AjaxNguyen.Core.Service;

namespace AjaxNguyen.Core.Manager
{
    public class AuthManager : PersistentSingleton<AuthManager>
    {
        private bool eventInitiated = false;
        private bool isSigningIn = false;

        JsonDataService dataService;

        // public bool IsLoggedIn => AuthenticationService.Instance.IsSignedIn;

        protected override async void Awake()
        {
            await InitializeUGS(); 

            dataService = new();
        }

        // private void OnDestroy()
        // {
        //     if (AuthenticationService.Instance != null)
        //     {
        //         AuthenticationService.Instance.SignedIn -= OnSignedIn;
        //         AuthenticationService.Instance.SignedOut -= OnSignedOut;
        //         AuthenticationService.Instance.Expired -= OnExpired;
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

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    PanelManager.Instance.OpenPanel("main");
                    Debug.Log("main panel");
                }
                else if (AuthenticationService.Instance.SessionTokenExists)
                {
                    await SignInAnonymousAsync();
                }
                else
                {
                    PanelManager.Instance.OpenPanel("auth");
                    Debug.Log("auth panel");
                }

            }
            catch (Exception e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.StartSevice, "Failed to conect to network ");
                Debug.Log(e.Message);
            }

        }

        public async Task SignInAnonymousAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PanelManager.Instance.OpenPanel("main");
            }
            catch (AuthenticationException e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "Failed to sign in");
                Debug.Log(e.Message);
            }
            catch (RequestFailedException e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "Failed to conect to network ");
                Debug.Log(e.Message);
            }
            catch (Exception e)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.None, "unexpected error occurred ");
                Debug.LogError($"An unexpected error occurred: {e.Message}");
            }

        }

        public async Task SignInWithUsernameAndPasswordAsync(string username, string password)
        {
            if (AuthenticationService.Instance.IsSignedIn) return;

            if (isSigningIn)
            {
                Debug.LogWarning("A sign-in process is already in progress.");
                return;
            }
            isSigningIn = true; // Đặt cờ để ngăn các lần gọi tiếp theo

            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
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
            try
            {
                if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
                {
                    await AuthenticationService.Instance.UpdatePlayerNameAsync("Anonymous Player");
                    PanelManager.Instance.OpenPanel("main");
                    Debug.Log("Player name: " + AuthenticationService.Instance.PlayerName);
                }
            }
            catch (AuthenticationException)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.SignIn, "Failed to sign in");
            }
            catch (RequestFailedException)
            {
                PanelManager.Instance.ShowErrorPopup(ErrorPopup.Action.SignIn, "Failed to conect to network ");
            }
        }
    }

}