using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
