using Ships;
using UnityEngine;

namespace ObjectsPool
{
    public class ShipPool : PoolBase<Ship>
    {
        protected override Ship CreateObject()
        {
            var ship = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            ship.gameObject.transform.SetParent(transform);
            return ship;
        }

        protected override void GetObject(Ship t)
        {
            t.gameObject.SetActive(true);
        }

        protected override void ReleaseObject(Ship t)
        {
            t.gameObject.SetActive(false);
        }
    }
}
