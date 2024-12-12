using Explosions;
using UnityEngine;

namespace ObjectsPool
{
    public class ExplosionFXPool : PoolBase<ExplosionFX>
    {
        protected override ExplosionFX CreateObject()
        {
            var explosion = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            explosion.gameObject.transform.SetParent(transform);
            return explosion;
        }

        protected override void GetObject(ExplosionFX t)
        {
            t.gameObject.SetActive(true);
        }

        protected override void ReleaseObject(ExplosionFX t)
        {
            t.gameObject.SetActive(false);
        }
    }
}
