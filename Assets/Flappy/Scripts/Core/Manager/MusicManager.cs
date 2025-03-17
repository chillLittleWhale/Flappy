using System.Collections;
using AjaxNguyen.Core.Audio;
using AjaxNguyen.Utility;
using UnityEngine;
using UnityEngine.Audio;

namespace Flappy.Core.Manager
{
    public class MusicManager : PersistentSingleton<MusicManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private SoundLibrary musicLibrary;
        [SerializeField] private AudioMixer audioMixer;

        private float volume = 0f;

        void Start()
        {
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 1f));
        }

        public void SetMusicVolume(float value)
        {
            volume = Mathf.Log10(value) * 20f;
            audioMixer.SetFloat("musicVolume", volume);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        public void PlayMusic(string trackName, float fadeDuration = 0.25f)
        {
            StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetAudioClip(trackName), fadeDuration));
        }

        /// <summary>
        /// Fades out the current music track, switches to a new one, and fades it back in.
        /// </summary>
        private IEnumerator AnimateMusicCrossfade(AudioClip newTrack, float fadeDuration = 0.5f)
        {
            float percent = 0f;
            while (percent < 1f)
            {
                percent += Time.deltaTime / fadeDuration;
                musicSource.volume = Mathf.Lerp(1f, 0f, percent);
                yield return null;
            }

            musicSource.clip = newTrack;
            musicSource.Play();

            percent = 0f;
            while (percent < 1f)
            {
                percent += Time.deltaTime / fadeDuration;
                musicSource.volume = Mathf.Lerp(0f, 1f, percent);
                yield return null;
            }
        }
    }
}
