using TMPro;
using UnityEngine;

namespace AjaxNguyen.Core.UI
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