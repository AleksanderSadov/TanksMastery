using UnityEngine;
using UnityEngine.AI;

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

            if (currentTarget != null)
            {
                shooting.CalculateOptimalForceForTarget(currentTarget);
            }

            if (!shooting.isCharging)
            {
                shooting.StartCharging();
            }

            if (shooting.isCharging)
            {
                shooting.IncrementCharge();
            }
        }

        public void TryNewAttackPosition()
        {
            if (!movement.HasReachedDestination())
            {
                return;
            }

            float attackRange = sightSensor.attackRange;
            Vector3 randomDirection = new Vector3(Random.Range(attackRange / 2, attackRange), transform.position.y, Random.Range(attackRange / 2, attackRange));
            float randomSign = Random.Range(0f, 1f) >= 0.5f ? 1f : -1f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(currentTarget.transform.position + randomDirection * randomSign, out hit, sightSensor.attackRange, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                movement.SetNavDestination(hit.position);
            }
        }
    }
}
