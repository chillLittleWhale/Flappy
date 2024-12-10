using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class PanelController : MonoBehaviour
    {
        public GameObject panel;

        public void OpenPanel()
        {
            panel.gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            panel.gameObject.SetActive(false);
        }
    }
}
