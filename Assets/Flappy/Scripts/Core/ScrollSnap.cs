using AjaxNguyen.Event;
using Flappy.Core.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core
{
    public class ScrollSnap : MonoBehaviour
    {
        [SerializeField] private IntEventChanel CurrentUIIndexChanged;
        public ScrollRect scrollRect;
        public RectTransform contentPanel;
        public RectTransform prefabItem;  // item mẫu để lấy width cho tính toán
        public HorizontalLayoutGroup h_LayoutGroup;

        private int itemCount = 0;
        private float contentTargetPosX;

        [SerializeField] float scrollThreshold = 50f; // tốc độ scroll bé hơn ngưỡng này thì xử lý
        [SerializeField] float snapSpeed = 10f; // Tốc độ snap

        [SerializeField] bool isScrolling = false;
        [SerializeField] int currentIndex = 0;  // index của item hiện tại trong vùng chọn
        private int tmpCurrentIndex = 0;
        private int rawIndex = 0;


        void Start()
        {
            itemCount = SkinManager.Instance.data.skinList.Count;  // TODO: vi phạm nghiêm trọng solid
            SetUpContentPading();
        }

        void Update()
        {
            rawIndex = Mathf.RoundToInt((GetCurrentCenterPos() - (h_LayoutGroup.spacing + h_LayoutGroup.padding.left)) / (prefabItem.rect.width + h_LayoutGroup.spacing));
            tmpCurrentIndex = Mathf.Clamp(rawIndex, 0, itemCount - 1);

            if (tmpCurrentIndex != currentIndex)
            {
                currentIndex = tmpCurrentIndex;
                CurrentUIIndexChanged.Raise(currentIndex);
            }

            if (isScrolling && scrollRect.velocity.magnitude < scrollThreshold)
            {
                scrollRect.velocity = Vector2.zero; // dừng cuộn

                contentTargetPosX = -(currentIndex * (prefabItem.rect.width + h_LayoutGroup.spacing) + (h_LayoutGroup.spacing + h_LayoutGroup.padding.left) - contentPanel.rect.width * 0.5f);

                contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, new Vector2(contentTargetPosX, contentPanel.anchoredPosition.y), Time.deltaTime * snapSpeed);

                if (contentPanel.anchoredPosition.x == contentTargetPosX)
                {
                    isScrolling = false;
                }
            }

            else if (!isScrolling && scrollRect.velocity.magnitude > scrollThreshold)
            {
                isScrolling = true;
            }
        }

        private float GetCurrentCenterPos()  // điểm trung tâm trên tọa độ trục x của content panel
        {
            return contentPanel.rect.width * 0.5f - contentPanel.anchoredPosition.x;
        }

        private void SetUpContentPading() // nếu horizontal padding (left, right) của content + spacing không bằng đúng 1 nửa của chiều rộng scrollView thì mọi thứ sẽ bị lệch
        {
            var scrollViewWidth = GetComponent<RectTransform>().rect.width;

            var horizontalPading = Mathf.RoundToInt(scrollViewWidth * 0.5f - h_LayoutGroup.spacing);

            h_LayoutGroup.padding.left = horizontalPading;
            h_LayoutGroup.padding.right = horizontalPading;
        }

    }

}
