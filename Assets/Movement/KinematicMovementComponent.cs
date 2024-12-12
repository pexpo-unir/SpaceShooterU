using UnityEngine;

namespace Movement
{
    public class KinematicMovementComponent : MovementComponentBase
    {
        protected override void Movement()
        {
            transform.Translate(Speed * Time.deltaTime * Direction);
        }
    }
}
