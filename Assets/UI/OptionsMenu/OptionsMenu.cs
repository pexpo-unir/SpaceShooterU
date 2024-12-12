using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider musicVolumeSlider;

        [SerializeField]
        private Slider sfxVolumeSlider;

        [SerializeField]
        private Toggle autoShootToggle;

        [SerializeField]
        private Toggle hasImmunityToggle;

        /// <summary>
        ///  Set the value of <see cref="musicVolumeSlider"/>.
        /// </summary>
        /// <param name="musicVolume">Normalized music value.</param>
        public void SetMusicVolumeValueSlider(float musicVolume)
        {
            musicVolumeSlider.value = musicVolume;
        }

        /// <summary>
        /// Set the value of <see cref="sfxVolumeSlider"/>.
        /// </summary>
        /// <param name="sfxVolume">Normalized sound effects value.</param>
        public void SetSFXVolumeValueSlider(float sfxVolume)
        {
            sfxVolumeSlider.value = sfxVolume;
        }

        public void SetAutoShootToggle(bool autoShoot)
        {
            autoShootToggle.isOn = autoShoot;
        }

        public void SetHasImmunityToggle(bool hasImmunity)
        {
            hasImmunityToggle.isOn = hasImmunity;
        }
    }
}
