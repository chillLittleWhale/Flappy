using System;
using AjaxNguyen.Core.UI;
using Flappy.Core.Manager;
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
        // [SerializeField] private string leaderboardId = "Flappy_HighestScore_Leaderboard";
        [SerializeField] Transform content; // Content của ScrollView
        [SerializeField] LeaderboardItemUI leaderboardItemPrefab;
        [SerializeField] LeaderboardItemUI selfScore;

        private LeaderboardScoresPage data;


        protected override void OnShow()
        {
            base.OnShow();
            GetSelfScoreAsync();
            GetLeaderboardScoreAsync();
        }

        private async void GetSelfScoreAsync()
        {
            var selfEntry = await LeaderBoardManager.Instance.GetSelfScoreAsync();
            selfScore.Initialize(selfEntry);
        }

        private async void GetLeaderboardScoreAsync()
        {
            try
            {
                data = await LeaderBoardManager.Instance.GetLeaderboardScoreAsync();
                if (data != null)
                {
                    ClearContent();
                    FillContent();
                }
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
