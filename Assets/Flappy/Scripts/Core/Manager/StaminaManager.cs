using UnityEngine;
using System;
using AjaxNguyen.Utility;

namespace Flappy.Core.Manager
{
    public class StaminaManager : PersistentSingleton<StaminaManager>
    {

        public event EventHandler<string> OnStaminaChanged; 
        [SerializeField]  int MAX_STAMINA = 36;               // Thể lực tối đa
        [SerializeField]  int RECOVERY_TIME_SECONDS = 60;      // Thời gian để hồi 1 thể lực
        private const string STAMINA_KEY = "CurrentStamina";
        private const string LAST_UPDATE_KEY = "LastUpdateTime";

        [SerializeField] int currentStamina;     // Thể lực hiện tại
        [SerializeField] double lastUpdateTime;  // Thời gian cập nhật cuối (Unix timestamp)

        [SerializeField] float updateInterval = 1f;  // Kiểm tra hồi phục mỗi 1 giây
        private float timer = 0f;

        void Start()
        {
            LoadStamina();
            RegenerateStamina();  // Cập nhật lại thể lực khi vào game
            SaveStamina();
            OnStaminaChanged?.Invoke(this, currentStamina + "/" + MAX_STAMINA);
        }

        void Update()
        {
            if (currentStamina == MAX_STAMINA) return;

            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                RegenerateStamina();
            }
        }

        void OnApplicationQuit()
        {
            SaveStamina();
        }

        // Tải dữ liệu từ PlayerPrefs
        private void LoadStamina()
        {
            currentStamina = PlayerPrefs.GetInt(STAMINA_KEY, MAX_STAMINA);
            // Lấy thời gian cập nhật cuối, nếu chưa có dùng thời gian hiện tại
            lastUpdateTime = PlayerPrefs.GetFloat(LAST_UPDATE_KEY, (float)GetCurrentUnixTimestamp());
        }

        // Lưu dữ liệu vào PlayerPrefs
        private void SaveStamina()
        {
            PlayerPrefs.SetInt(STAMINA_KEY, currentStamina);
            PlayerPrefs.SetFloat(LAST_UPDATE_KEY, (float)lastUpdateTime);
            PlayerPrefs.Save();
        }

        // Hàm cập nhật thể lực dựa trên thời gian đã trôi qua
        private void RegenerateStamina()
        {
            double currentTime = GetCurrentUnixTimestamp();
            double elapsedSeconds = currentTime - lastUpdateTime;

            // Nếu thời gian âm (do thay đổi đồng hồ hệ thống), reset lastUpdateTime
            if (elapsedSeconds < 0)
            {
                lastUpdateTime = currentTime;
                SaveStamina();
                return;
            }

            int staminaToAdd = (int)(elapsedSeconds / RECOVERY_TIME_SECONDS);
            if (staminaToAdd > 0 && currentStamina < MAX_STAMINA)
            {
                currentStamina = Mathf.Min(currentStamina + staminaToAdd, MAX_STAMINA);
                // Điều chỉnh lastUpdateTime cho phần dư chưa đủ để hồi phục thêm
                lastUpdateTime = currentTime - (elapsedSeconds % RECOVERY_TIME_SECONDS);
                SaveStamina();
                Debug.Log($"Stamina regenerated: {currentStamina}/{MAX_STAMINA}");

                OnStaminaChanged?.Invoke(this, GetStaminaStatus());
            }
        }

        // Sử dụng thể lực khi chơi game
        public bool TrySpendStamina(int amount)
        {
            // Cập nhật hồi phục trước khi tiêu thụ thể lực
            RegenerateStamina();
            if (currentStamina >= amount)
            {
                currentStamina -= amount;
                SaveStamina();
                return true;
            }
            return false;
        }

        //nhận thêm thể lực
        public void AddStamina(int amount)
        {
            currentStamina = Mathf.Min(currentStamina + amount, MAX_STAMINA);
            SaveStamina();
            OnStaminaChanged?.Invoke(this, GetStaminaStatus());
        }

        // Trả về thể lực hiện tại
        public int GetCurrentStamina()
        {
            RegenerateStamina();
            return currentStamina;
        }

        // Lấy Unix timestamp hiện tại
        private double GetCurrentUnixTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public string GetStaminaStatus() => currentStamina + "/" + MAX_STAMINA;
    }

}
