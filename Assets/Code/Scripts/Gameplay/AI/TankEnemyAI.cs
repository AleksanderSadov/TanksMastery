using UnityEngine;

namespace Tanks.Gameplay
{
    [RequireComponent(typeof(TankEnemyController))]
    public class TankEnemyAI : MonoBehaviour
    {
        public enum AIState
        {
            FindTarget,
            Follow,
            Attack,
        }

        public AIState aiState;

        private TankEnemyController enemyController;

        private void Start()
        {
            enemyController = GetComponent<TankEnemyController>();
            aiState = AIState.FindTarget;
        }

        private void Update()
        {
            UpdateAiStateTransitions();
            UpdateCurrentAiState();
        }

        private void UpdateAiStateTransitions()
        {
            switch (aiState)
            {
                case AIState.FindTarget:
                    if (enemyController.currentTarget != null)
                    {
                        aiState = AIState.Follow;
                    }
                    break;
                case AIState.Follow:
                    if (enemyController.isTargetInAttackRange)
                    {
                        aiState = AIState.Attack;
                    }
                    break;
                case AIState.Attack:
                    if (!enemyController.isTargetInAttackRange)
                    {
                        aiState = AIState.FindTarget;
                    }
                    break;
            }
        }

        private void UpdateCurrentAiState()
        {
            switch (aiState)
            {
                case AIState.FindTarget:
                    enemyController.FindTarget();
                    break;
                case AIState.Follow:
                    enemyController.FollowTarget(enemyController.currentTarget);
                    break;
                case AIState.Attack:
                    enemyController.OrientTowards(enemyController.currentTarget.transform.position);
                    enemyController.TryAttack();
                    enemyController.TryNewAttackPosition();
                    break;
            }
        }
    }
}
