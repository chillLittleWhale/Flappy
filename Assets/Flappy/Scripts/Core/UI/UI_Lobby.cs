using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class UI_Lobby : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Button mapButton;
        [SerializeField] private Button playButton;
        [SerializeField] private int staminaCost = 1;
        [SerializeField] PlayerInfoData data;

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

        public void FirstReload()  // quá trình đăng nhập làm cho data được set và trong các Manager chậm hơn, hàm Start của các UIPanel chưa có dữ liệu chuẩn để hiển thị, nên phải Update lần đầu bằng event riêng
        {
            data = PlayerInfoManager.Instance.data;
            Reload(this, data);
        }

        void OnDestroy()
        {
            PlayerInfoManager.Instance.OnPlayerInfoDataChanged -= Reload;
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

        // private async void DelayUIUpdate()
        // {
        //     await Task.Delay(100);
        //     Reload(this, data);
        // }
    }
}
