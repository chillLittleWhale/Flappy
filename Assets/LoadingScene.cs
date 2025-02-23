using System.Collections;
using System.Threading.Tasks;
using AjaxNguyen.Event;
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

    }
}
