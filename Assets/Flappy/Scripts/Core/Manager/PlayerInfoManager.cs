using System;
using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using UnityEngine;


namespace Flappy.Core.Manager
{
    public class PlayerInfoManager : PersistentSingleton<PlayerInfoManager>
    {
        public event EventHandler<PlayerInfoData> OnPlayerInfoDataChanged; //--
        public PlayerInfoData data;
        private PlayerInfoData tempData;  // when save load error occurs, this will make sure the real data is not be effected


        public async void ChangePlayerName(string name)
        {
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(name);

                tempData = new(data)
                {
                    playerName = name
                };
                TrySaveData_Local(tempData);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

        }

        private void TrySaveData_Local(PlayerInfoData data)
        {
            if (SaveLoadManager.Instance.TrySaveData_Local(data, "PlayerInfoData"))
            {
                this.data = data;
                OnPlayerInfoDataChanged?.Invoke(this, data);
            }
            else Debug.LogWarning("Save PlayerInfo data fail");
        }

        public void SetData(PlayerInfoData data)
        {
            this.data = data;
        }

    }


    [Serializable]
    public struct PlayerInfoData
    {
        public string playerName;
        public string playerPassword;

        public PlayerInfoData(PlayerInfoData data)
        {
            playerName = data.playerName;
            playerPassword = data.playerPassword;
        }

    }
}
