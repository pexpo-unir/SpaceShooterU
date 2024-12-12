using Ships;
using UnityEngine;

namespace GameController
{
    public class KillEnemy : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (gameObject.CompareTag(collider.gameObject.tag))
            {
                return;
            }

            if (collider.gameObject.TryGetComponent<Ship>(out var ship))
            {
                ship.DamageComponent.GetDamage(ship.DamageComponent.Health);
            }
        }
    }
}
