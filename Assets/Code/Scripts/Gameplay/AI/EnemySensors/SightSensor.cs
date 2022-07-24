using UnityEngine;

namespace Tanks.Gameplay
{
    public class SightSensor : MonoBehaviour
    {
        public float attackRange = 20.0f;
        public float attackAngle = 20.0f;

        public bool isTargetInAttackRange;
        public bool isTargetInAttackAngle;

        private TankEnemyController enemyController;

        private void Start()
        {
            enemyController = GetComponent<TankEnemyController>();
        }

        private void FixedUpdate()
        {
            CheckTargetInSight();
        }

        protected virtual void CheckTargetInSight()
        {
            if (enemyController.currentTarget == null)
            {
                return;
            }

            Vector3 direction = (enemyController.currentTarget.transform.position - transform.position).normalized;
            float targetAngle = Vector3.Angle(direction, transform.forward);

            isTargetInAttackRange = Vector3.Distance(transform.position, enemyController.currentTarget.transform.position) <= attackRange;
            isTargetInAttackAngle = targetAngle < attackAngle;
        }
    }
}
