using System.Collections;
using DamageSystem;
using Explosions;
using Movement;
using ObjectsPool;
using UnityEngine;
using Weapons;
using Lasers;

namespace Ships
{
    [RequireComponent(typeof(KinematicMovementComponent))]
    [RequireComponent(typeof(DamageComponent))]
    public class Ship : MonoBehaviour, IPoolableObject<Ship>
    {
        [SerializeField]
        private ShipData shipData;

        private SpriteRenderer spriteRenderer;

        public MovementComponentBase MovementComponent { get; private set; }

        public DamageComponent DamageComponent { get; private set; }

        [SerializeField]
        private float flashDamageDuration = 0.1f;

        private WeaponBase weaponBase;

        [SerializeField]
        private GameObject weaponSlot;

        [SerializeField]
        private Color originalColor = Color.white;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            MovementComponent = GetComponent<KinematicMovementComponent>();
            DamageComponent = GetComponent<DamageComponent>();
            DamageComponent.OnDeath += Death;
        }

        void Start()
        {
            // Preconfigured in editor.
            if (shipData != null)
            {
                LoadData(shipData);
            }
        }

        void OnEnable()
        {
            spriteRenderer.color = originalColor;
        }

        public void LoadData(ShipData shipData)
        {
            Debug.Assert(shipData != null, $"Variable {nameof(shipData)} cannot be null.");

            this.shipData = shipData;

            gameObject.tag = shipData.Tag;

            spriteRenderer.sprite = shipData.Sprite;
            MovementComponent.Speed = shipData.Speed;

            float angle = Mathf.Atan2(shipData.FacingDirection.y, shipData.FacingDirection.x) * Mathf.Rad2Deg - 90; // TODO: (0, 1) = 0 deg (not 90 deg)
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);

            ChangeWeapon(shipData.WeaponData);

            DamageComponent.MaxHealth = shipData.MaxHealth;

            spriteRenderer.color = originalColor;
        }

        public void Move(Vector2 direction)
        {
            MovementComponent.Direction = direction;
        }

        public void ChangeWeapon(WeaponData weaponData)
        {
            weaponBase = Instantiate(weaponData.weaponBase, weaponSlot.transform);
            weaponBase.transform.SetParent(weaponSlot.transform);
            weaponBase.LoadData(weaponData);

            ChangeLaser(shipData.LaserData);
        }

        public void ChangeLaser(LaserData laserData)
        {
            weaponBase.ChangeLaser(laserData);
        }

        public void ShootLaser()
        {
            weaponBase.Fire();
        }

        private void Death()
        {
            var pooledExplosion = ExplosionFXPool.Instance.Get().GetComponent<ExplosionFX>();
            pooledExplosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            if (gameObject.CompareTag("Enemy"))
            {
                ReturnToPool();
            }
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

        private IEnumerator FlashRed()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDamageDuration);
            spriteRenderer.color = originalColor;
        }

        public void ReturnToPool()
        {
            if (isActiveAndEnabled)
            {
                ShipPool.Instance.Release(this);
            }
        }
    }
}
