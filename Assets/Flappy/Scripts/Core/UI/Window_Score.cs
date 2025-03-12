using TMPro;
using UnityEngine;

namespace Flappy.Core.UI
{
    public class Window_Score : MonoBehaviour
    {
        private TextMeshProUGUI scoreText;
        private TextMeshProUGUI highestScoreText;

        void Awake()
        {
            scoreText = transform.Find("CurrentScore").GetComponent<TextMeshProUGUI>();
            highestScoreText = transform.Find("HighestScore").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            highestScoreText.text = "HighestScore: " + PlayerPrefs.GetInt("HighestScore", 0).ToString();
        }

        private void OnEnable()
        {
            Level.Instance.OnScoreChanged += UpdateUI;
        }

        private void OnDisable()
        {
            Level.Instance.OnScoreChanged -= UpdateUI;
        }

        private void UpdateUI(object sender, int e)
        {
            scoreText.text = e.ToString();
        }
    }
}