using ObjectsPool;
using Lasers;
using UnityEngine;

namespace Weapons
{
    public class DoubleWeapon : WeaponBase
    {
        [field: SerializeField]
        public GameObject LeftMuzzle { get; set; }

        [field: SerializeField]
        public GameObject RightMuzzle { get; set; }

        public override void Fire()
        {
            if (FireInCooldown) return;

            StartCooldown();

            PlayFireSound();

            var pooledLaser = LaserPool.Instance.Get().GetComponent<Laser>();
            pooledLaser.transform.SetPositionAndRotation(LeftMuzzle.transform.position, LeftMuzzle.transform.rotation);
            pooledLaser.LoadData(LaserData);

            pooledLaser = LaserPool.Instance.Get().GetComponent<Laser>();
            pooledLaser.transform.SetPositionAndRotation(RightMuzzle.transform.position, RightMuzzle.transform.rotation);
            pooledLaser.LoadData(LaserData);
        }
    }
}
