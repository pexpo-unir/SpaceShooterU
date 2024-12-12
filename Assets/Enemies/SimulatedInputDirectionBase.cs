using UnityEngine;

namespace Enemies
{
    public abstract class SimulatedInputDirectionBase : MonoBehaviour
    {
        public abstract Vector2 SimulatePressedDirection();
    }
}
