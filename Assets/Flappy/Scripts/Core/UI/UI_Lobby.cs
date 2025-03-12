using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen.Core.UI
{
    public class UI_Lobby : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Button mapButton;
        [SerializeField] private Button playButton;
        [SerializeField] private int staminaCost = 1;
        private PlayerInfoData data;

        void Awake()
        {
            playButton.onClick.AddListener(OnClickPlayBtn);
        }

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
            if (StaminaManager.Instance.GetCurrentStamina() < staminaCost)
            {
                PanelManager.Instance.ShowErrorPopup("Not enough stamina");
                return;
            }

            if (StaminaManager.Instance.TrySpendStamina(staminaCost))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }
        }
    }
}
