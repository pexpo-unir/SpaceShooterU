using System.Collections;
using UnityEngine;
using DamageSystem;
using Weapons;
using Lasers;

namespace Turrets
{
    [RequireComponent(typeof(DamageComponent))]
    public class Turret : MonoBehaviour
    {
        public delegate void Destroyed();
        public event Destroyed OnDestroyed;

        [SerializeField]
        private TurretData turretData;

        [SerializeField]
        private GameObject pivot;

        private SpriteRenderer spriteRenderer;

        private WeaponBase weaponBase;

        [SerializeField]
        private GameObject weaponSlot;

        public DamageComponent DamageComponent { get; private set; }

        [SerializeField]
        private GameObject explosionVFX;

        [SerializeField]
        private float turretShootAngle;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            DamageComponent = GetComponent<DamageComponent>();
            DamageComponent.OnDeath += Destroy;

            originalColor = spriteRenderer.color;
        }

        void Start()
        {
            // Preconfigured in editor.
            if (turretData != null)
            {
                LoadData(turretData);
            }

            StartCoroutine(ShootCoroutine());
        }

        public void LoadData(TurretData turretData)
        {
            Debug.Assert(turretData != null, $"Variable {nameof(turretData)} cannot be null.");

            this.turretData = turretData;

            gameObject.tag = turretData.Tag;

            spriteRenderer.sprite = turretData.Sprite;

            float angle = Mathf.Atan2(turretData.FacingDirection.y, turretData.FacingDirection.x) * Mathf.Rad2Deg - 90; // (0, 1) = 0 deg (not 90 deg)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);

            weaponBase = Instantiate(turretData.WeaponData.weaponBase, weaponSlot.transform);
            weaponBase.transform.SetParent(weaponSlot.transform);
            weaponBase.LoadData(turretData.WeaponData);
            weaponBase.ChangeLaser(turretData.LaserData);

            DamageComponent.MaxHealth = turretData.MaxHealth;
        }

        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                float rndRotation = Random.Range(-turretShootAngle, turretShootAngle);
                pivot.transform.localRotation = Quaternion.Euler(0, 0, rndRotation);
                ShootLaser();

                yield return new WaitForSeconds(turretData.ShootRate);
            }
        }

        private void Destroy()
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            DamageComponent.OnDeath -= Destroy; // move to OnDisable??
            OnDestroyed?.Invoke();
            gameObject.SetActive(false);
        }

        public void ShootLaser()
        {
            weaponBase.Fire();
        }

        private Color originalColor;
        private IEnumerator FlashRed()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (gameObject.CompareTag(collider.gameObject.tag))
            {
                return;
            }

            if (collider.gameObject.TryGetComponent<Laser>(out var laser))
            {
                laser.Destroy();

                if (isActiveAndEnabled)
                {
                    StopCoroutine(FlashRed());
                    StartCoroutine(FlashRed());

                    DamageComponent.GetDamage(laser.Damage);
                }
            }
        }
    }
}
