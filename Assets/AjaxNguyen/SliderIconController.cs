using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen
{
    public class SliderIconController : MonoBehaviour
    {
        [SerializeField] private Sprite nomalSprite;
        [SerializeField] private Sprite minValueSprite;
        [SerializeField] private Image image;
        [SerializeField] private float minValue;

        public void SetSliderIcon(float value)
        {
            if (value <= minValue)
            {
                image.sprite = minValueSprite;
            }
            else
            {
                image.sprite = nomalSprite;
            }
        }
    }
}
