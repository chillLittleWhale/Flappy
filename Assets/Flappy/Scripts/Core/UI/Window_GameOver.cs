using System;
using DG.Tweening;
using Flappy.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class Window_GameOver : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        [SerializeField] private GameObject panel;

        void Awake()
        {
            if (scoreText == null) scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
            if (highScoreText == null) highScoreText = transform.Find("HighestScore").GetComponent<TextMeshProUGUI>();
            if (panel == null) panel = transform.Find("Panel").gameObject;

            retryButton.onClick.AddListener(RetryBtnOnClick);
            menuButton.onClick.AddListener(MenuBtnOnClick);
        }

        void Start()
        {
            Hide();
        }

        void OnEnable()
        {
            Level.Instance.OnStateChange += GameOverWindow_OnStateChange;
        }

        void OnDestroy()
        {
            Level.Instance.OnStateChange -= GameOverWindow_OnStateChange;
        }

        private void GameOverWindow_OnStateChange(object sender, GameState e)
        {
            if (e == GameState.GameOver)
            {
                scoreText.text = Level.Instance.GetPlayerScore().ToString();
                highScoreText.text = "Highest Score: " + Level.Instance.GetHighestScore().ToString();
                Show();

                panel.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack);
            }
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        public void RetryBtnOnClick()
        {
            if (StaminaManager.Instance.TrySpendStamina(1))
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                SceneManager.LoadScene("MenuScene");
            }
        }

        public void MenuBtnOnClick()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
