using DamageSystem;
using Movement;
using Lasers;
using Turrets;
using UnityEngine;
using System.Collections;

namespace Bosses
{
    [RequireComponent(typeof(KinematicMovementComponent))]
    [RequireComponent(typeof(DamageComponent))]
    public class Boss : MonoBehaviour
    {
        [SerializeField]
        private Turret[] turrets;
        private int turretsRemaining;

        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private float flashDamageDuration = 0.1f;

        [SerializeField]
        private Color originalColor = Color.white;

        public MovementComponentBase MovementComponent { get; private set; }

        public DamageComponent DamageComponent { get; private set; }

        [SerializeField]
        private GameObject explosionVFX;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            MovementComponent = GetComponent<KinematicMovementComponent>();
            DamageComponent = GetComponent<DamageComponent>();
            DamageComponent.OnDeath += Death;
            DamageComponent.HasImmunity = true;

            turretsRemaining = turrets.Length;
        }

        void Start()
        {
            foreach (var turret in turrets)
            {
                turret.OnDestroyed += () =>
                {
                    turretsRemaining--;
                    if (turretsRemaining == 0)
                    {
                        DamageComponent.HasImmunity = false;
                    }
                };
            }
        }

        private void Death()
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            DamageComponent.OnDeath -= Death; // move to OnDisable??
            gameObject.SetActive(false);
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
                    if (!DamageComponent.HasImmunity)
                    {
                        StopCoroutine(FlashRed());
                        StartCoroutine(FlashRed());
                    }

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
    }
}
