using UnityEngine;

namespace DamageSystem
{
    public class DamageComponent : MonoBehaviour
    {
        public delegate void HealthChanged(int oldValue, int newValue);
        public event HealthChanged OnHealthChanged;

        public delegate void Death();
        public event Death OnDeath;

        [SerializeField]
        private int _health = 1;
        public int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, MaxHealth);
            }
        }

        [SerializeField]
        private int _maxHealth = 1;
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
                _health = _maxHealth;
                GetHealing(_health);
            }
        }

        [field: SerializeField]
        public bool HasImmunity { get; set; } = false;

        public void GetDamage(int amount)
        {
            if (HasImmunity)
            {
                return;
            }

            var oldHealthValue = _health;
            Health -= amount;
            OnHealthChanged?.Invoke(oldHealthValue, Health);

            if (Health == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void GetHealing(int amount)
        {
            var oldHealthValue = _health;
            Health += amount;
            OnHealthChanged?.Invoke(oldHealthValue, Health);
        }
    }
}
