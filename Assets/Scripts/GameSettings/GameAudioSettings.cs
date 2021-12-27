using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

namespace ProjectN.Settings
{
    [CreateAssetMenu(fileName = "GameAudioSettings", menuName = "ProjectN/Settings/AudioSettings")]
    public class GameAudioSettings : ScriptableObject
    {
        public float masterVolume = 1.0f;
        public float musicVolume = 1.0f;
        public float sfxVolume = 1.0f;
        public AudioMixerGroup masterAudioMixer;
        public AudioMixerGroup musicAudioMixer;
        public AudioMixerGroup sfxAudioMixer;
        public List<AudioClipInfo> audioClips;
        public List<AudioClipInfo> musicAudioClips;

        [Serializable]
        public class AudioClipInfo
        {
            public string clipName;
            public float volume;
            public AudioClip clip;
        }
    }
}