using TMPro;
using UnityEngine;

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
        Level.GetInstance().OnScoreChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Level.GetInstance().OnScoreChanged -= UpdateUI;
    }

    private void UpdateUI(object sender, int e)
    {
        scoreText.text = e.ToString();
    }
}
