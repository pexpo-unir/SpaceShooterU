using ObjectsPool;
using Lasers;
using UnityEngine;

namespace Weapons
{
    public class CircularWeapon : WeaponBase
    {
        [field: SerializeField]
        public GameObject Muzzle { get; set; }

        public override void Fire()
        {
            if (FireInCooldown) return;

            StartCooldown();

            PlayFireSound();

            float degrees = 360 / weaponData.ShotsNumber;

            for (float i = 0; i < 360; i += degrees)
            {
                Muzzle.transform.eulerAngles = Vector3.forward * i;

                var pooledLaser = LaserPool.Instance.Get().GetComponent<Laser>();
                pooledLaser.transform.SetPositionAndRotation(Muzzle.transform.position, Muzzle.transform.rotation);
                pooledLaser.LoadData(LaserData);
            }
        }
    }
}
