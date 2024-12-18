using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class UI_Lobby : MonoBehaviour
    {
        public void OnClickPlayBtn ()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene ("GameScene");
        }
    }
}
