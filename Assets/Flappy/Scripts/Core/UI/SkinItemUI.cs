using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class SkinItemUI : MonoBehaviour
    {
        public Image skinImage;
        public TextMeshProUGUI skinName;

        private Skin skinData;

        public void Initialize(Skin data)
        {
            skinData = data;

            skinImage.sprite = skinData.skinIcon; // sprite là hình ảnh của skin
            skinName.text = skinData.skinName; // name là tên của skin
        }
    }
}
