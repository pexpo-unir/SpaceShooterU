using Lasers;
using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField]
        protected WeaponData weaponData;

        /// <summary>
        /// The time between weapon shots.
        /// </summary>
        /// /// <remarks>
        /// Measured in seconds.
        /// </remarks>
        private float fireRate = 1f;

        /// <summary>
        /// Internal timer for <see cref="fireRate"/>.
        /// </summary>
        private float fireCooldown;

        public bool FireInCooldown => fireCooldown > 0;

        [Header("Fire sfx")]
        [SerializeField]
        private AudioClip[] audioClips;
        private AudioSource audioSource;

        [SerializeField]
        private float minPitch = 1.5f;

        [SerializeField]
        private float maxPitch = 2f;

        protected LaserData LaserData { get; private set; }

        protected virtual void Awake()
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        public abstract void Fire();

        public void ChangeLaser(LaserData laserData)
        {
            LaserData = laserData;
        }

        protected void Update()
        {
            fireCooldown -= 1 * Time.deltaTime;
        }

        protected void StartCooldown()
        {
            fireCooldown = fireRate;
        }

        public void LoadData(WeaponData weaponData)
        {
            Debug.Assert(weaponData != null, "Variable weaponData cannot be null.");
            this.weaponData = weaponData;

            fireRate = weaponData.FireRate;
        }

        protected void PlayFireSound()
        {
            var rndPitch = Random.Range(minPitch, maxPitch);
            audioSource.pitch = rndPitch;

            var rndClip = Random.Range(0, audioClips.Length);
            var clip = audioClips[rndClip];
            audioSource.PlayOneShot(clip);
        }
    }
}
