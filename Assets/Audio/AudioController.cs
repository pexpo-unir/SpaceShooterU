using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Audio Mixer Params

        public readonly string masterLowpassAudioMixerParam = "MasterLowpassSimple";
        public readonly string musicVolumeAudioMixerParam = "MusicVolume";
        public readonly string sfxVolumeAudioMixerParam = "SFXVolume";

        #endregion

        [SerializeField]
        private AudioMixer gameAudioMixer;

        [SerializeField]
        private float audioMixerLowpassHz = 512f;
        private float initialAudioMixerLowpassHz;

        void Awake()
        {
            Debug.Assert(gameAudioMixer != null, $"Variable {nameof(gameAudioMixer)} cannot be null.");

            gameAudioMixer.GetFloat(masterLowpassAudioMixerParam, out initialAudioMixerLowpassHz);
        }

        public void SetMusicVolume(float musicVolume)
        {
            gameAudioMixer.SetFloat(musicVolumeAudioMixerParam, NormalizedVolumeToDbVolume(musicVolume));
        }

        public void SetSFXVolume(float sfxVolume)
        {
            gameAudioMixer.SetFloat(sfxVolumeAudioMixerParam, NormalizedVolumeToDbVolume(sfxVolume));
        }

        public void ApplyLowpassFilter()
        {
            gameAudioMixer.SetFloat(masterLowpassAudioMixerParam, audioMixerLowpassHz);
        }

        public void RemoveLowpassFilter()
        {
            gameAudioMixer.SetFloat(masterLowpassAudioMixerParam, initialAudioMixerLowpassHz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float NormalizedVolumeToDbVolume(float normalizedVolume) => Mathf.Log10(normalizedVolume) * 20;
    }
}
