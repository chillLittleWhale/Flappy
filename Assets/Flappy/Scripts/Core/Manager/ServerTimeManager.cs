using System;
using System.Net.Http;
using System.Threading.Tasks;
using AjaxNguyen.Utility;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Flappy.Core.Manager
{
    public class ServerTimeManager : PersistentSingleton<ServerTimeManager>
    {
        private const string API_URL = "https://timeapi.io/api/Time/current/zone?timeZone=UTC";

        // public async Task<DateTime> GetServerTimeAsync()
        // {
        //     using HttpClient client = new HttpClient();
        //     try
        //     {
        //         HttpResponseMessage response = await client.GetAsync(API_URL);
        //         if (response.IsSuccessStatusCode)
        //         {
        //             string json = await response.Content.ReadAsStringAsync();
        //             WorldTimeApiResponse timeData = JsonUtility.FromJson<WorldTimeApiResponse>(json);
        //             return DateTime.Parse(timeData.datetime).ToLocalTime();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"Failed to get server time: {e.Message}");
        //     }
        //     return DateTime.Now; // Fallback nếu có lỗi
        // }

        public async Task<DateTime> GetServerTimeAsync()
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequest.Get(API_URL))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        throw new System.Exception($"Failed to fetch server time: {request.error}");
                    }

                    string jsonResult = request.downloadHandler.text;
                    Debug.Log($"Raw JSON response: {jsonResult}");

                    // Parse JSON bằng Newtonsoft.Json
                    var timeData = JsonConvert.DeserializeObject<TimeApiResponse>(jsonResult);

                    // Chuyển đổi trường dateTime từ JSON thành DateTime
                    if (DateTime.TryParse(timeData.dateTime, out DateTime serverTime))
                    {
                        Debug.Log($"Server time parsed: {serverTime}");
                        return serverTime;
                    }
                    else
                    {
                        throw new System.Exception("Failed to parse server time from JSON response");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error fetching server time: {e.Message}");
                // Fallback: Trả về DateTime.Now nếu có lỗi (không khuyến khích vì có thể bị gian lận)
                return DateTime.Now;
            }
        }

        // [Serializable]
        // private class WorldTimeApiResponse
        // {
        //     public string datetime;
        // }


        private class TimeApiResponse
        {
            public string dateTime { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public int day { get; set; }
            public int hour { get; set; }
            public int minute { get; set; }
            public int seconds { get; set; }
            public string timeZone { get; set; }
        }
    }

}
