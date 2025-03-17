using System;
using System.Collections;
using System.Threading.Tasks;
using AjaxNguyen.Core.UI;
using AjaxNguyen.Event;
using AjaxNguyen.Utility.Event;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AjaxNguyen
{
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] GameObject loadingBar;
        [SerializeField] Slider loadingSlider;
        [SerializeField] TextMeshProUGUI loadingText;

        private float progress = 0f;

        void Start()
        {
            HandleAthenPanel();
        }

        private async void HandleAthenPanel()
        {
            await Task.Delay(1000);
            if (loadingBar.activeSelf) return;  // nếu đã load thì không hiện Authen Panel nữa

            PanelManager.Instance.OpenPanel(PanelType.Authen);
        }

        public void Load()
        {
            StartCoroutine(HandleLoading());
        }

        public IEnumerator HandleLoading()
        {
            loadingSlider.value = 0f;
            loadingText.text = "0%";
            loadingBar.SetActive(true);

            var operation = SceneManager.LoadSceneAsync("MenuScene");

            while (!operation.isDone)
            {
                progress = Mathf.Clamp01(operation.progress / .9f); // phải là 0.9 chứ không phải 1, vì Unity dùng 90% de load scene, 10% để mở scene khi operation.allowSceneActivation = true.
                loadingSlider.value = progress;
                loadingText.text = progress * 100f + "%";

                yield return null;
            }
        }


        // public async Task HandleLoading2()
        // {
        //     Debug.Log("HandleLoading2");
        //     loadingSlider.value = 0f;
        //     loadingText.text = "0%";
        //     loadingBar.SetActive(true);

        //     var operation = SceneManager.LoadSceneAsync("MenuScene");

        //     do
        //     {
        //         progress = Mathf.Clamp01(operation.progress / .9f); // phải là 0.9 chứ không phải 1, vì Unity dùng 90% de load scene, 10% để mở scene khi operation.allowSceneActivation = true.
        //         loadingSlider.value = progress;
        //         loadingText.text = progress * 100f + "%";

        //     }
        //     while (operation.progress < 0.9f);

        //     await Task.Delay(1000);
        // }

    }
}
