using Flappy.Core.Manager;
using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class PanelController : MonoBehaviour
    {
        public void OpenPanel(GameObject panel) => panel.gameObject.SetActive(true); 
        public void ClosePanel(GameObject panel) => panel.gameObject.SetActive(false);

        public void OpenPanel(int panelIndex) => PanelManager.Instance.OpenPanel((PanelType)panelIndex);
        public void ClosePanel(int panelIndex) => PanelManager.Instance.ClosePanel((PanelType)panelIndex);

    }
}
