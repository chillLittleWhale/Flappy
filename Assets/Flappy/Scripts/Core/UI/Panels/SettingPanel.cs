using Flappy.Core.Manager;
using AjaxNguyen.Core.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using AjaxNguyen;
using AjaxNguyen.Utility.Event;

namespace Flappy.Core.Panels
{
    public class SettingPanel : Panel
    {
        [SerializeField] Button logoutButton;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;


        public override void Awake()
        {
            base.Awake();

            logoutButton.onClick.AddListener(SignOut);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            // test async event bus
            // AsyncEventBus.Subscribe<AsyncEBusTest>(HandleSignOutAsync);
        }

        // private async Task HandleSignOutAsync(AsyncEBusTest msg)  // test
        // {
        //     await Task.Delay(1000);
        //     Debug.Log("Test async event bus log");
        // }

        // private async Task Lmao() // test
        // {
        //     await AsyncEventBus.Publish(new AsyncEBusTest());
        // }

        void Start()
        {
            musicSlider.value = FromVolumnToValue(PlayerPrefs.GetFloat("musicVolume", 1f));
            sfxSlider.value = FromVolumnToValue(PlayerPrefs.GetFloat("sfxVolume", 1f));
            MusicManager.Instance.PlayMusic("MuoiNganNam");  //TODO:  tạm để đây, chỉnh sau
        }

        private async void SignOut()
        {
            Debug.LogWarning("SettingPanel: SignOut");
            await AuthManager.Instance.SignOut();
        }

        private void OnMusicVolumeChanged(float value)
        {
            MusicManager.Instance.SetMusicVolume(value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            SfxManager.Instance.SetSFXVolume(value);
        }


        public void OnTestSFXClick()
        {
            SfxManager.Instance.PlaySfx("ButtonClick", transform.position);
        }

        /// <summary>
        /// Converts a volume level from decibels to a linear value.
        /// </summary>
        private float FromVolumnToValue(float volume)
        {
            return Mathf.Pow(10f, volume / 20f);
        }
    }
}
