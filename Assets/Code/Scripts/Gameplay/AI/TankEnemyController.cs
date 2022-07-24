using UnityEngine;

namespace Tanks.Gameplay
{
    [RequireComponent(typeof(TankEnemyMovement))]
    [RequireComponent(typeof(TankShooting))]
    [RequireComponent(typeof(FindTargetSensor))]
    public class TankEnemyController : MonoBehaviour
    {
        public GameObject currentTarget;

        public bool isTargetInAttackRange => sightSensor.isTargetInAttackRange;
        public bool isTargetInAttackAngle => sightSensor.isTargetInAttackAngle;

        private TankEnemyMovement movement;
        private TankShooting shooting;
        private FindTargetSensor findTargetSensor;
        private SightSensor sightSensor;

        private void Start()
        {
            movement = GetComponent<TankEnemyMovement>();
            shooting = GetComponent<TankShooting>();
            findTargetSensor = GetComponent<FindTargetSensor>();
            sightSensor = GetComponent<SightSensor>();
        }

        private void Update()
        {
            if (!isTargetInAttackRange || !isTargetInAttackRange)
            {
                shooting.ResetCharge();
            }
        }

        public void FindTarget()
        {
            currentTarget = findTargetSensor.FindRandomTarget();
        }

        public void FollowTarget(GameObject target)
        {
            movement.SetNavDestination(target.transform.position);
        }

        public void OrientTowards(Vector3 lookPosition)
        {
            movement.OrientTowards(lookPosition);
        }

        public void TryAttack()
        {
            if (!isTargetInAttackRange || !isTargetInAttackAngle)
            {
                return;
            }

            if (!shooting.isCharging)
            {
                shooting.targetLaunchForce = 30.0f;
                shooting.StartCharging();
            }

            if (shooting.isCharging)
            {
                shooting.IncrementCharge();
            }
        }
    }
}
