using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


namespace Flappy.Core.Panels
{
    public class ChangeNamePopUp : Panel
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button closeButton;

        [SerializeField] PlayerInfoData data;


        public override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(OnClickCloseButton);
            confirmButton.onClick.AddListener(OnClickConfirmButton);
        }

        void Start()
        {
            PlayerInfoManager.Instance.OnPlayerInfoDataChanged += Reload;
            data = PlayerInfoManager.Instance.data; // binding data
            inputField.text = data.playerName;
        }

        void OnClickCloseButton()
        {
            PanelManager.Instance.ClosePanel(PanelType.ChangeNamePopUp);
        }

        void OnClickConfirmButton()
        {
            // Debug.Log("OnClickConfirmButton");
            var inputName = RemoveWhitespace(inputField.text);
            PlayerInfoManager.Instance.ChangePlayerName(inputName);
        }

        public void Reload(object sender, PlayerInfoData e)
        {
            inputField.text = e.playerName;
        }

        public void FirstReload()  // quá trình đăng nhập làm cho data được set và trong các Manager chậm hơn, hàm Start của các UIPanel chưa có dữ liệu chuẩn để hiển thị, nên phải Update lần đầu bằng event riêng
        {
            data = PlayerInfoManager.Instance.data;
            Reload(this, data);
        }

        private string RemoveWhitespace(string input)  // TODO: cho vào utility
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }
}
