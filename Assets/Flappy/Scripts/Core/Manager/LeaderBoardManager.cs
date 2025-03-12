using System;
using System.Threading.Tasks;
using AjaxNguyen.Utility;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace Flappy.Core.Manager
{
    public class LeaderBoardManager : PersistentSingleton<LeaderBoardManager>
    {

        [SerializeField] private string leaderboardId = "Flappy_HighestScore_Leaderboard";
        private LeaderboardScoresPage data;

        public async void AddScoreAsync(int score)
        {
            try
            {
                await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
                // await GetLeaderboardScoreAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to add player score: {ex.Message}");
            }
        }

        public async Task<LeaderboardEntry> GetSelfScoreAsync()
        {
            try
            {
                var selfEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
                // selfScore.Initialize(selfEntry);
                return selfEntry;

            }
            catch (LeaderboardsException e)
            {
                if (e.Reason == LeaderboardsExceptionReason.EntryNotFound) // Xử lý trường hợp người chơi chưa có điểm trong leaderboard
                {
                    Debug.LogWarning("Player has no score in leaderboard yet. Displaying N/A.");
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

            var defaultEntry = new LeaderboardEntry(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName ?? "Anonymous", -1, 0);
            return defaultEntry;
        }

        public async Task<LeaderboardScoresPage> GetLeaderboardScoreAsync()
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

                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get leaderboard scores: {e.Message}");
                return null;
            }
        }
    }
}
