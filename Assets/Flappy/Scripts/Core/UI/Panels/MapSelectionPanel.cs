using Flappy.Core.Manager;
using AjaxNguyen.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.Panels
{
    public class MapSelectionPanel : Panel
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

        void OnDisable()
        {
            MapManager.Instance.OnMapDataChanged -= Reload;
        }

        public void FirstReload()  // quá trình đăng nhập làm cho data được set và trong các Manager chậm hơn, hàm Start của các UIPanel chưa có dữ liệu chuẩn để hiển thị, nên phải Update lần đầu bằng event riêng
        {
            Reload(this, data);
        }

        void Reload(object sender, MapData e)
        {
            currentMapIndex = data.mapList.FindIndex(map => map.id == data.selectedMapID);
            ReloadUI();
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
