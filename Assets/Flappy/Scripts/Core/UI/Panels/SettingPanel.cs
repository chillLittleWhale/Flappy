using Flappy.Core.Manager;
using AjaxNguyen.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Flappy.Core.Panels
{
    public class SettingPanel : Panel
    {
        [SerializeField] Button logoutButton;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;


        public override void Initialize()
        {
            base.Initialize();

            logoutButton.onClick.AddListener(SignOut);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        void Start()
        {
            musicSlider.value = FromVolumnToValue(PlayerPrefs.GetFloat("musicVolume", 1f));
            sfxSlider.value = FromVolumnToValue(PlayerPrefs.GetFloat("sfxVolume", 1f));
            MusicManager.Instance.PlayMusic("MuoiNganNam");  //TODO:  tạm để đây, chỉnh sau
        }

        private async void SignOut()
        {
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
