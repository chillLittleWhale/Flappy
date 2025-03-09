using System;
using AjaxNguyen.Core.UI;
using Flappy.Core.UI;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.Panels
{
    public class RankPanel : Panel
    {
        //ref
        [SerializeField] private string leaderboardId = "Flappy_HighestScore_Leaderboard";
        [SerializeField] Transform content; // Content của ScrollView
        [SerializeField] Button addScroreButton;
        [SerializeField] LeaderboardItemUI leaderboardItemPrefab;
        [SerializeField] LeaderboardItemUI selfScore;

        private LeaderboardScoresPage data;


        protected override void OnShow()
        {
            base.OnShow();
            GetSelfScoreAsync();
            GetLeaderboardScoreAsync();
        }

        public override void Awake()
        {
            base.Awake();
            addScroreButton.onClick.AddListener(AddScoreAsync);
        }

        public async void AddScoreAsync()
        {
            try
            {
                var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, 12);
                GetLeaderboardScoreAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add player score: {ex.Message}");
            }
        }


        private async void GetSelfScoreAsync()
        {
            try
            {
                var selfEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
                selfScore.Initialize(selfEntry);

            }
            catch (LeaderboardsException e)
            {
                if (e.Reason == LeaderboardsExceptionReason.EntryNotFound) // Xử lý trường hợp người chơi chưa có điểm trong leaderboard
                {
                    Debug.LogWarning("Player has no score in leaderboard yet. Displaying N/A.");

                    var defaultEntry = new LeaderboardEntry( AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName ?? "Anonymous", -1, 0);

                    selfScore.Initialize(defaultEntry);
                }
                else
                {
                    // Xử lý các lỗi khác (ví dụ: lỗi mạng, chưa xác thực, v.v.)
                    Debug.LogError($"Failed to get current player score due to other error: {e.Message}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get current player score: {e.Message}");
            }
        }

        private async void GetLeaderboardScoreAsync()
        {
            try
            {
                data = await LeaderboardsService.Instance.GetScoresAsync(
                    leaderboardId,
                    new GetScoresOptions
                    {
                        Offset = 0, // bắt đầu từ index 0
                        Limit = 10, // Lấy 10 player
                    });

                ClearContent();
                FillContent();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get leaderboard scores: {e.Message}");
            }
        }

        private void ClearContent()
        {
            // Debug.Log("ClearContent");
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }

        private void FillContent()
        {
            // Debug.Log("FillContent");
            for (int i = 0; i < data.Results.Count; i++)
            {
                LeaderboardItemUI item = Instantiate(leaderboardItemPrefab, content);
                item.Initialize(data.Results[i]);
            }
        }
    }
}
