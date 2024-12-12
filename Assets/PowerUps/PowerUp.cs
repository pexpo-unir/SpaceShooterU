using System;
using System.Collections;
using Movement;
using Ships;
using UnityEngine;

namespace PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField]
        private PowerUpData powerUpData;

        private SpriteRenderer spriteRenderer;

        private MovementComponentBase movementComponent;

        private AudioSource audioSource;

        [SerializeField]
        private float floatSpeed = 2f;

        [SerializeField]
        private float floatAmplitude = 1f;

        private Vector3 initialPosition;
        private float initialOffset;

        // TODO: Create Timer like Godot?
        public float lifeSpanDuration = 5f;
        private float lifeSpan;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            movementComponent = GetComponent<KinematicMovementComponent>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        void Start()
        {
            initialPosition = transform.position;
            initialOffset = UnityEngine.Random.Range(0, 2 * Mathf.PI);

            if (powerUpData != null)
            {
                LoadData(powerUpData);
            }
        }

        private void CheckLifeSpanTimer()
        {
            if (lifeSpan >= lifeSpanDuration)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            float newY = initialPosition.y + Mathf.Cos(Time.time * floatSpeed + initialOffset) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            lifeSpan += Time.deltaTime;
            CheckLifeSpanTimer();
        }

        public void LoadData(PowerUpData powerUpData)
        {
            Debug.Assert(powerUpData != null, $"Variable {nameof(powerUpData)} cannot be null.");

            this.powerUpData = powerUpData;

            spriteRenderer.sprite = powerUpData.Sprite;
            movementComponent.Speed = powerUpData.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (gameObject.CompareTag(collider.gameObject.tag))
            {
                return;
            }

            if (collider.gameObject.TryGetComponent<Ship>(out var ship))
            {
                if (!collider.gameObject.CompareTag("Player"))
                {
                    return;
                }

                powerUpData.ApplyEffect(ship);
                spriteRenderer.enabled = false;
                StartCoroutine(AudioSourcePlayAndWait(() =>
                {
                    Destroy(gameObject);
                }));
            }
        }

        private void OnEnable()
        {
            lifeSpan = 0;
        }

        private IEnumerator AudioSourcePlayAndWait(Action audioSourceFinish)
        {
            audioSource.PlayOneShot(audioSource.clip);

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            audioSourceFinish?.Invoke();
        }
    }
}
