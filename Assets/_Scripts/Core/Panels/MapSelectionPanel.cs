using System.Linq;
using AjaxNguyen.Core.Manager;
using AjaxNguyen.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AjaxNguyen.Core.Panels
{
    public class MapSelectionPanel : MonoBehaviour
    {
        [SerializeField] MapData data;
        [SerializeField] int currentMapIndex;
        [SerializeField] Image mapImage;
        [SerializeField] TextMeshProUGUI mapName;

        void Start()
        {
            MapManager.Instance.OnMapDataChanged += Reload;

            data = MapManager.Instance.data; // binding data

            currentMapIndex = data.mapList.FindIndex(map => map.id == data.selectedMapID);

            // hiển thị current map

            Reload(this, data);

        }

        // void OnEnable()
        // {
        //     Reload(this, data);
        // }

        void Reload(object sender, MapData e)
        {
            currentMapIndex = data.mapList.FindIndex(map => map.id == data.selectedMapID);

            mapImage.sprite = data.mapList[currentMapIndex].mapIcon;
            mapName.text = data.mapList[currentMapIndex].mapName;

        }

        void ReloadUI()
        {
            mapImage.sprite = data.mapList[currentMapIndex].mapIcon;
            mapName.text = data.mapList[currentMapIndex].mapName;
        }

        public void OnButtonClick_Select()
        {
            data.selectedMapID = data.mapList[currentMapIndex].id;
            MapManager.Instance.ReSelectMap(data.mapList[currentMapIndex].id);
        }

        public void OnButtonClick_Next(int index)
        {
            currentMapIndex = (index + currentMapIndex + data.mapList.Count) % data.mapList.Count;
            ReloadUI();
        }
    }
}
