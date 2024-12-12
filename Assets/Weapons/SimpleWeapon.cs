using ObjectsPool;
using Lasers;
using UnityEngine;

namespace Weapons
{
    public class SimpleWeapon : WeaponBase
    {
        [field: SerializeField]
        public GameObject Muzzle { get; set; }

        public override void Fire()
        {
            if (FireInCooldown) return;

            StartCooldown();

            PlayFireSound();

            var pooledLaser = LaserPool.Instance.Get().GetComponent<Laser>();
            pooledLaser.transform.SetPositionAndRotation(Muzzle.transform.position, Muzzle.transform.rotation);
            pooledLaser.LoadData(LaserData);
        }
    }
}
