using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen.Core.UI
{
    public class MapItemUI : MonoBehaviour
    {
        public Image mapImage;
        public TextMeshProUGUI mapName;

        private Map mapData;

        public void Setup(Map data)
        {
            mapData = data;

            mapImage.sprite = mapData.mapIcon; 
            mapName.text = mapData.mapName; 
        }
    }
}
