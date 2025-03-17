using System.Linq;
using Flappy.Core.Manager;
using AjaxNguyen.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Flappy.Core.UI;

namespace Flappy.Core.Panels
{
    public class SkinSelectionPanel : Panel
    {
        //ref
        [SerializeField] Transform content; // Content của ScrollView
        [SerializeField] GameObject skinItemPrefab; // Prefab của mỗi skin
        [SerializeField] TextMeshProUGUI skinCount_Text;
        [SerializeField] GameObject iconSelected;

        public RectTransform contentPanel;
        public RectTransform prefabItem;  // item mẫu để lấy width cho tính toán
        public HorizontalLayoutGroup h_LayoutGroup;

        [SerializeField] SkinData data;

        private int unlockedSkinCount = 1;

        void Start()
        {
            SkinManager.Instance.OnSkinDataChanged += Reload;

            data = SkinManager.Instance.data; // data binding 

            Reload(this, data);
        }

        protected override void OnShow()
        {
            Reload(this, data);
            SnapToChild(data.skinList.FindIndex(skin => skin.id == data.selectedSkinID));
        }

        void OnDisable()
        {
            SkinManager.Instance.OnSkinDataChanged -= Reload;
        }

        public void FirstReload()  // quá trình đăng nhập làm cho data được set và trong các Manager chậm hơn, hàm Start của các UIPanel chưa có dữ liệu chuẩn để hiển thị, nên phải Update lần đầu bằng event riêng
        {
            Reload(this, data); //TODO: không hiểu sao đoạn code này có vẻ bị thừa?
        }

        public void Reload(object sender, SkinData e)
        {
            PopulateItems();
            UpdateUI_SkinCount();
        }

        void PopulateItems()
        {
            // Xóa các item cũ trong Content nếu có
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            // Tạo các item và gán dữ liệu
            // foreach (Skin skin in data.skinList)
            // {
            //     GameObject item = Instantiate(skinItemPrefab, content);
            //     SkinItemUI skinItemUI = item.GetComponent<SkinItemUI>();

            //     skinItemUI.Initialize(skin);
            // }
            for(int i = 0; i < data.skinList.Count; i++) 
            {
                GameObject item = Instantiate(skinItemPrefab, content);
                SkinItemUI skinItemUI = item.GetComponent<SkinItemUI>();

                skinItemUI.Initialize(data.skinList[i], i);
            }
        }

        void UpdateUI_SkinCount()
        {
            unlockedSkinCount = data.skinList.Count(skin => skin.isUnlocked == true);

            skinCount_Text.text = unlockedSkinCount.ToString() + "/" + data.skinList.Count.ToString();
        }

        /// <summary>
        /// Calculate the index of the skin nearest to the center of the panel, then select it.
        /// </summary>
        public void OnButtonClick_Select()
        {
            var rawIndex = Mathf.RoundToInt((GetCurrentCenterPos() - (h_LayoutGroup.spacing + h_LayoutGroup.padding.left)) / (prefabItem.rect.width + h_LayoutGroup.spacing));
            int finalIndex = Mathf.Clamp(rawIndex, 0, data.skinList.Count - 1);
            var newSelectedId = data.skinList[finalIndex].id;

            if (newSelectedId == data.selectedSkinID) return;

            if (SkinManager.Instance.TrySelectSkin(newSelectedId))
            {
                data.selectedSkinID = newSelectedId;
            }

        }

        private float GetCurrentCenterPos()  // điểm trung tâm trên tọa độ trục x của content panel
        {
            return contentPanel.rect.width * 0.5f - contentPanel.anchoredPosition.x;
        }

        /// <summary>
        /// Snap the content panel to the position of the child at the given index, so that the child is in the center of the panel.
        /// </summary>
        /// <param name="index">The index of the child to snap to.</param>
        /// <param name="childNumber">The total number of children.</param>
        private void SnapToChild(int index)
        {   // vì khi start thì contentPanel.rect.width đang bằng 0 do child của nó sẽ bị clear bởi hàm PopulateItems của CharacterPanel, nên phải tự tính
            int childNumber = data.skinList.Count;
            var contentPanelWith = h_LayoutGroup.padding.left * 2f + childNumber * prefabItem.rect.width + (childNumber - 1) * h_LayoutGroup.spacing;
            var targetPosX = -(index * (prefabItem.rect.width + h_LayoutGroup.spacing) + (h_LayoutGroup.spacing + h_LayoutGroup.padding.left) - contentPanelWith * 0.5f);

            contentPanel.anchoredPosition = new Vector2(targetPosX, contentPanel.anchoredPosition.y);
        }



        #region EventChanel callback
        public void OnUiItemClicked(int index) => SnapToChild(index); 

        public void OnCurrentUiItemChange(int index)
        {
            if (data.selectedSkinID == index.ToString())
            {
                iconSelected.SetActive(true);
            }
            else
            {
                iconSelected.SetActive(false);
            }
        }
        #endregion
    }
}
