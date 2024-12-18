using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class PanelController : MonoBehaviour
    {
        // public GameObject panel;

        public void OpenPanel(GameObject panel) => panel.gameObject.SetActive(true); 
        public void ClosePanel(GameObject panel) => panel.gameObject.SetActive(false);
    }
}
