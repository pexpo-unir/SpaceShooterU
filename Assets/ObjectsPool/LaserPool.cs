using Lasers;
using UnityEngine;

namespace ObjectsPool
{
    public class LaserPool : PoolBase<Laser>
    {
        protected override Laser CreateObject()
        {
            var laser = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            laser.gameObject.transform.SetParent(transform);
            return laser;
        }

        protected override void GetObject(Laser t)
        {
            t.gameObject.SetActive(true);
        }

        protected override void ReleaseObject(Laser t)
        {
            t.gameObject.SetActive(false);
        }
    }
}
