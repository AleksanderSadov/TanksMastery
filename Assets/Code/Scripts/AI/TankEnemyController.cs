using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.AI
{
    [RequireComponent(typeof(TankEnemyMovement))]
    [RequireComponent(typeof(TankShooting))]
    [RequireComponent(typeof(FindTargetSensor))]
    public class TankEnemyController : MonoBehaviour
    {
        public GameObject currentTarget;

        private TankEnemyMovement movement;
        private TankShooting shooting;
        private FindTargetSensor findTargetSensor;

        private void Start()
        {
            movement = GetComponent<TankEnemyMovement>();
            shooting = GetComponent<TankShooting>();
            findTargetSensor = GetComponent<FindTargetSensor>();
        }

        public void FindTarget()
        {
            currentTarget = findTargetSensor.FindRandomTarget();
        }

        public void FollowTarget(GameObject target)
        {
            movement.SetNavDestination(target.transform.position);
        }
    }
}
