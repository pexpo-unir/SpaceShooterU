using System.Collections;
using Ships;
using UI.HUD;
using UnityEngine;

namespace Players
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void PlayerDies();
        public static event PlayerDies OnPlayerDies;

        private InputSystem_Actions inputActions;

        [SerializeField]
        private Ship ship;

        [SerializeField]
        private HUD hud;

        public bool AutoShoot { get; set; } = false;

        private bool _hasImmunity;
        public bool HasImmunity
        {
            get => _hasImmunity;
            set
            {
                _hasImmunity = value;
                ship.DamageComponent.HasImmunity = value;
            }
        }

        void Awake()
        {
            inputActions = new();
        }

        void Start()
        {
            if (ship != null)
            {

                ship.DamageComponent.OnHealthChanged += (oldValue, newValue) =>
                {
                    hud.UpdateHealth(oldValue, newValue);
                };

                ship.DamageComponent.GetHealing(ship.DamageComponent.MaxHealth);

                ship.DamageComponent.OnDeath += () =>
                {
                    StartCoroutine(WaitForSecondsAndInvokePlayerDies(.666f));
                };
            }
        }

        void Update()
        {
            if (ship == null || !ship.isActiveAndEnabled)
            {
                return;
            }

            var moveDirection = inputActions.Player.Move.ReadValue<Vector2>();
            ship.Move(moveDirection);

            if (AutoShoot)
            {
                ship.ShootLaser();
            }
            else if (inputActions.Player.Shoot.IsPressed())
            {
                ship.ShootLaser();
            }
        }

        private void OnEnable()
        {
            inputActions.Enable();
            ship.gameObject.SetActive(true);
            ship.DamageComponent.GetHealing(ship.DamageComponent.MaxHealth);
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private IEnumerator WaitForSecondsAndInvokePlayerDies(float sec)
        {
            ship.gameObject.SetActive(false);
            yield return new WaitForSeconds(sec);
            OnPlayerDies?.Invoke();
        }
    }
}
