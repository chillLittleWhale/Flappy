using System.Linq;
using AjaxNguyen.Core.Manager;
using AjaxNguyen.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen.Core.Panels
{
    public class SkinSelectionPanel : MonoBehaviour
    {
        //ref
        [SerializeField] Transform content; // Content của ScrollView
        [SerializeField] GameObject skinItemPrefab; // Prefab của mỗi skin
        [SerializeField] TextMeshProUGUI skinCount_Text;

        public RectTransform contentPanel;
        public RectTransform prefabItem;  // item mẫu để lấy width cho tính toán
        public HorizontalLayoutGroup h_LayoutGroup;

        [SerializeField] SkinData data;

        private int unlockedSkinCount = 1;

        void Start()
        {
            SkinManager.Instance.OnSkinDataChanged += Reload;

            data = SkinManager.Instance.data; // binding data
            PopulateItems();
            UpdateUI_SkinCount();

            SnapToChild(data.skinList.FindIndex(skin => skin.id == data.selectedSkinID), data.skinList.Count);
        }

        void OnEnable()
        {
            SnapToChild(data.skinList.FindIndex(skin => skin.id == data.selectedSkinID), data.skinList.Count);
        }

        void Reload(object sender, SkinData e)
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
            foreach (Skin skin in data.skinList)
            {
                GameObject item = Instantiate(skinItemPrefab, content);
                SkinItemUI skinItemUI = item.GetComponent<SkinItemUI>();

                skinItemUI.Setup(skin);
            }
        }

        void UpdateUI_SkinCount()
        {
            unlockedSkinCount = data.skinList.Count(skin => skin.isUnlocked == true);

            skinCount_Text.text = unlockedSkinCount.ToString() + "/" + data.skinList.Count.ToString();
        }

        public void OnButtonClick_Select()
        {
            var rawIndex = Mathf.RoundToInt((GetCurrentCenterPos() - (h_LayoutGroup.spacing + h_LayoutGroup.padding.left)) / (prefabItem.rect.width + h_LayoutGroup.spacing));
            int finalIndex = Mathf.Clamp(rawIndex, 0, data.skinList.Count -1);
            var newSelectedId = data.skinList[finalIndex].id;

            if ( newSelectedId == data.selectedSkinID ) return;

            data.selectedSkinID = newSelectedId;
            SkinManager.Instance.ReSelectSkin(newSelectedId);

        }

        private float GetCurrentCenterPos()  // điểm trung tâm trên tọa độ trục x của content panel
        {
            return contentPanel.rect.width * 0.5f - contentPanel.anchoredPosition.x;
        }

        private void SnapToChild(int index, int childNumber)
        {   // vì khi start thì contentPanel.rect.width đang bằng 0 do child của nó sẽ bị clear bởi hàm PopulateItems của CharacterPanel, nên phải tự tính
            var contentPanelWith = h_LayoutGroup.padding.left * 2f + childNumber * prefabItem.rect.width + (childNumber - 1) * h_LayoutGroup.spacing;
            var targetPosX = -(index * (prefabItem.rect.width + h_LayoutGroup.spacing) + (h_LayoutGroup.spacing + h_LayoutGroup.padding.left) - contentPanelWith * 0.5f);

            contentPanel.anchoredPosition = new Vector2(targetPosX, contentPanel.anchoredPosition.y);
        }
    }
}
