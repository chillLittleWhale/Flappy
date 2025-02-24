using AjaxNguyen.Core.Audio;
using AjaxNguyen.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace Flappy.Core.Manager
{
    public class SfxManager : PersistentSingleton<SfxManager>
    {
        [SerializeField] private AudioSource SFX_prefab;
        [SerializeField] private SoundLibrary sfxLibrary;
        [SerializeField] private AudioMixer audioMixer;

        private float duration = 0f;
        private int randIndex = 0;
        private float volume = 0f;


        void Start()
        {
            audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 1f));
        }

        public void SetSFXVolume(float value)
        {
            volume = Mathf.Log10(value) * 20f;
            audioMixer.SetFloat("sfxVolume", volume);
            PlayerPrefs.SetFloat("sfxVolume", volume);
        }

        public void PlaySfx(string clipName, Vector3 pos, float volume = 1f)
        {
            PlaySfx(sfxLibrary.GetAudioClip(clipName), pos, volume);
        }

        public void PlaySfx(AudioClip audioClip, Vector3 pos, float volume = 1f)
        {
            var sfx = Instantiate(SFX_prefab, pos, Quaternion.identity);
            sfx.transform.SetParent(transform);

            sfx.clip = audioClip;
            sfx.volume = volume;
            sfx.Play();

            // Hủy sfx sau khi play hoàn tất
            duration = audioClip.length;
            Destroy(sfx.gameObject, duration);
        }

        public void PlaySfx_Random(AudioClip[] audioClips, Vector3 pos, float volume = 1f)
        {
            randIndex = Random.Range(0, audioClips.Length);
            PlaySfx(audioClips[randIndex], pos, volume);
        }
    }
}
