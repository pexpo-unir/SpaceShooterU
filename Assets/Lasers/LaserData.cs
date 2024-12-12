using UnityEngine;

namespace Lasers
{
    [CreateAssetMenu(fileName = "LaserData", menuName = "Lasers/LaserData")]
    public class LaserData : ScriptableObject
    {
        [field: SerializeField]
        public string Tag { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField]
        public int Damage { get; set; }
    }
}
