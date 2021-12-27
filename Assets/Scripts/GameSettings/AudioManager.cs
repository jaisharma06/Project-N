using UnityEngine;
using ProjectN.Settings;
using System.Collections.Generic;

namespace ProjectN.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private GameAudioSettings gameAudioSettings;
        [SerializeField]
        private AudioSource musicAudioSource;
        [SerializeField]
        private List<AudioSource> sfxAudioSources;

        public static AudioManager Instance { get; set; }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject, 0);
            }
        }

        private void Awake()
        {
            SetMasterVolume(gameAudioSettings.masterVolume);
            SetMasterVolume(gameAudioSettings.musicVolume);
            SetSfxVolume(gameAudioSettings.sfxVolume);
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetMasterVolume(float volume)
        {
            gameAudioSettings.masterAudioMixer.audioMixer.SetFloat("volume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            gameAudioSettings.musicAudioMixer.audioMixer.SetFloat("volume", volume);
        }

        public void SetSfxVolume(float volume)
        {
            gameAudioSettings.sfxAudioMixer.audioMixer.SetFloat("volume", volume);
        }

        public void PlaySFX()
        {

        }

        public void PlayMusic(string clipName)
        {
            GameAudioSettings.AudioClipInfo audioClipInfo = GetAudioClipInfo(clipName, true);
            musicAudioSource.clip = audioClipInfo.clip;
            musicAudioSource.Play();
        }

        private GameAudioSettings.AudioClipInfo GetAudioClipInfo(string clipName, bool isMusic = false)
        {
            if (isMusic)
            {
                return gameAudioSettings.musicAudioClips.Find(ac => ac.clipName == clipName);
            }
            else
            {
                return gameAudioSettings.audioClips.Find(ac => ac.clipName == clipName);
            }
        }
    }
}

