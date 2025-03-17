using AjaxNguyen.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class SkinItemUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] IntEventChanel OnSkinUIItemClick;
        [SerializeField] int index = 0;
        public Image skinImage;
        [SerializeField] Image lockImage;
        [SerializeField] GameObject lockIcon;
        public TextMeshProUGUI skinName;

        private Skin skinData;

        public void Initialize(Skin data, int index)
        {
            skinData = data;

            this.index = index;
            skinImage.sprite = skinData.skinIcon; // sprite là hình ảnh của skin
            lockImage.sprite = skinData.skinIcon;
            skinName.text = skinData.skinName; // name là tên của skin
            if (data.isUnlocked)
            {
                lockIcon.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSkinUIItemClick.Raise(index);
        }
    }
}
