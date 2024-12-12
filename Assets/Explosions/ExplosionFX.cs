using ObjectsPool;
using UnityEngine;

namespace Explosions
{
    public class ExplosionFX : MonoBehaviour, IPoolableObject<ExplosionFX>
    {
        private ParticleSystem vfx;

        void Awake()
        {
            vfx = GetComponentInChildren<ParticleSystem>();
        }

        void Update()
        {
            if (!vfx.IsAlive())
            {
                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            if (isActiveAndEnabled)
            {
                ExplosionFXPool.Instance.Release(this);
            }
        }
    }
}
