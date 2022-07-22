using System;
using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.AI;

namespace Tanks.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class TankEnemyMovement : TankMovement
    {
        public float turnSpeed = 180f;

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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                isTurning = true;
            }
        }

        public override bool IsMoving()
        {
            if (!navMeshAgent.isOnNavMesh)
            {
                return false;
            }

            if (
                !navMeshAgent.pathPending
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            )
            {
                return false;
            }

            return true;
        }

        public override bool IsTurning()
        {
            return isTurning;
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
