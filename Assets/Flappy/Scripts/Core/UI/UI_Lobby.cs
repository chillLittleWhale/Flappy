using Flappy.Core.Manager;
using TMPro;
using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class UI_Lobby : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        private PlayerInfoData data;


        void Start()
        {
            PlayerInfoManager.Instance.OnPlayerInfoDataChanged += Reload;

            data = PlayerInfoManager.Instance.data; // binding data

            Reload(this, data);
        }

        public void Reload(object sender, PlayerInfoData e)
        {
            playerNameText.text = e.playerName;
        }

        public void OnClickPlayBtn()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}
