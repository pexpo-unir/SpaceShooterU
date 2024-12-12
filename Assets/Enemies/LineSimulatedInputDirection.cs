using UnityEngine;

namespace Enemies
{
    public class LineSimulatedInputDirection : SimulatedInputDirectionBase
    {
        public override Vector2 SimulatePressedDirection()
        {
            return Vector2.left;
        }
    }
}
