using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Window_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private GameObject panel;

    void Awake()
    {
        if (scoreText == null) scoreText = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        if (highScoreText == null) highScoreText = transform.Find("HighestScore").GetComponent<TextMeshProUGUI>();
        if (panel == null) panel = transform.Find("Panel").gameObject;
    }

    void Start()
    {
        Hide();
    }

    void OnEnable()
    {
        Level.GetInstance().OnStateChange += GameOverWindow_OnStateChange;
    }

    void OnDestroy()
    {
        Level.GetInstance().OnStateChange -= GameOverWindow_OnStateChange;
    }

    private void GameOverWindow_OnStateChange(object sender, GameState e)
    {
        if (e == GameState.GameOver)
        {
            scoreText.text = Level.GetInstance().GetPlayerScore().ToString();
            Show();

            panel.transform.DOLocalMove(new Vector3(0,0,0), 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void Show() => gameObject.SetActive(true);

    private void Hide() => gameObject.SetActive(false);

    public void RetryBtnOnClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MenuBtnOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
