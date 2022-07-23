using UnityEngine;

namespace Tanks.Gameplay
{
    public abstract class TankMovement : MonoBehaviour
    {
        public abstract bool IsMoving();
        public abstract bool IsTurning();
        public abstract void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius);
    }
}
