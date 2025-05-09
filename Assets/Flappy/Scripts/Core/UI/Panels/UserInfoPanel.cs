using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.Panels
{
    public class UserInfoPanel : Panel
    {
        [SerializeField] private Button nameEditButton;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI tagNameText;

        [SerializeField] private PlayerInfoData data;

        public override void Awake()
        {
            base.Awake();
            nameEditButton.onClick.AddListener(OnClickNameEditButton);
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

        public void Reload(object sender, PlayerInfoData e)
        {
            playerNameText.text = e.playerName;
        }

        public void OnClickNameEditButton()
        {
            PanelManager.Instance.OpenPanel(PanelType.ChangeNamePopUp);
        }
    }
}
