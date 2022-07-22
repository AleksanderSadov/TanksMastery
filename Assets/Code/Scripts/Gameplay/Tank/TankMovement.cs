using UnityEngine;

namespace Tanks.Gameplay
{
    public abstract class TankMovement : MonoBehaviour
    {
        public virtual bool isMoving => isMoving;

        public abstract bool IsMoving();
    }
}
