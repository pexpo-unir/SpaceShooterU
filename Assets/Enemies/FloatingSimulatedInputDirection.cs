using UnityEngine;

namespace Enemies
{
    public class FloatingSimulatedInputDirection : SimulatedInputDirectionBase
    {
        [SerializeField]
        private float floatSpeed = 2f;

        [SerializeField]
        private float floatAmplitude = 1f;

        public override Vector2 SimulatePressedDirection()
        {
            float newY = Mathf.Cos(Time.time * floatSpeed) * floatAmplitude;
            return new Vector2(-1, newY);
        }
    }
}
