using UnityEngine;

namespace Tanks.Gameplay
{
    public abstract class TankMovement : MonoBehaviour
    {
        public abstract bool IsMoving();
        public abstract bool IsTurning();
    }
}
