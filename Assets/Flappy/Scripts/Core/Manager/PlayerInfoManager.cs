using System;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using UnityEngine;


namespace Flappy.Core.Manager
{
    public class PlayerInfoManager : PersistentSingleton<PlayerInfoManager>
    {
        public event EventHandler<PlayerInfoData> OnPlayerInfoDataChanged; //--
        public PlayerInfoData data = new();
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
            PlayerPrefs.SetInt("HighestScore", data.highestScore);
        }

    }


    [Serializable]
    public class PlayerInfoData
    {
        public string playerName;
        public string playerPassword;
        public int highestScore;

        //copy constructor
        public PlayerInfoData(PlayerInfoData data)
        {
            playerName = data.playerName;
            playerPassword = data.playerPassword;
            highestScore = data.highestScore;
        }

        public PlayerInfoData()
        {
            playerName = "New_Player";
            playerPassword = "DefaultPass1@";
            highestScore = 0;
        }
    }
}

