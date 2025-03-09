using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.UI
{
    public class LeaderboardItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private Image avatarImage;
        [SerializeField] private Image countryImage;
        private LeaderboardEntry player;


        public void Initialize(LeaderboardEntry player) 
        {
            this.player = player;
            nameText.text = player.PlayerName;
            scoreText.text = player.Score.ToString();
            rankText.text = (player.Rank +1).ToString();
            if (player.Rank == -1) rankText.text = "N/A";

            // avatarImage.sprite = ;  // Quả này khoai, phải lấy thông qua Id => public data, nhưng mà để sau
            // countryImage.sprite = ;
        }
    }
}
