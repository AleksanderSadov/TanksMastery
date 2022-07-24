using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Tanks.Gameplay
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankEnemyMovement : TankMovement
    {
        private bool isTurning = false;
        private NavMeshAgent navMeshAgent;
        private Rigidbody rigidBody;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            rigidBody.isKinematic = true;

            navMeshAgent.enabled = false;
            Vector3 spawnPosition;
            if (FindSpawnPosition(transform.position, out spawnPosition))
            {
                transform.position = spawnPosition;
                navMeshAgent.enabled = true;
            }
        }   

        public void SetNavDestination(Vector3 destination)
        {
            if (navMeshAgent && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(destination);
            }
        }

        public void OrientTowards(Vector3 lookPosition)
        {
            isTurning = false;

            Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
            if (lookDirection.sqrMagnitude != 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * navMeshAgent.angularSpeed);
                isTurning = true;
            }
        }

        public override bool IsMoving()
        {
            if (!navMeshAgent.isOnNavMesh)
            {
                return false;
            }

            if (HasReachedDestination())
            {
                return false;
            }

            return true;
        }

        public override bool IsTurning()
        {
            return isTurning;
        }

        public bool HasReachedDestination()
        {
            if 
                (navMeshAgent.enabled
                && !navMeshAgent.pathPending
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            )
            {
                return true;
            }

            return false;
        }

        public override void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            if (rigidBody.isKinematic)
            {
                rigidBody.isKinematic = false;
            }

            if (navMeshAgent.enabled)
            {
                navMeshAgent.enabled = false;
            }

            rigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);

            StartCoroutine(CheckExplosionForceEnded());
        }

        private IEnumerator CheckExplosionForceEnded()
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            yield return new WaitUntil(() => rigidBody.velocity.magnitude < 0.5f);

            if (!rigidBody.isKinematic)
            {
                rigidBody.isKinematic = true;
            }

            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }
        }

        private bool FindSpawnPosition(Vector3 position, out Vector3 result)
        {
            NavMeshHit hit;
            int maxDistance = 1;
            int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");
            if (NavMesh.SamplePosition(position, out hit, maxDistance, walkableAreaMask))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }
    }
}
