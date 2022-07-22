using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Gameplay
{
    public class CameraControl : MonoBehaviour
    {
        public float dampTime = 0.2f;
        public float screenEdgeBuffer = 4f;
        public float minSize = 6.5f;
        public Transform[] targets;

        private Camera mainCamera;
        private float zoomSpeed;
        private Vector3 moveVelocity;
        private Vector3 desiredPosition;
        private TeamManager teamManager;

        private void Awake()
        {
            mainCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
        }

        private void FixedUpdate()
        {
            SetCameraTargets();
            Move();
            Zoom();
        }

        private void SetCameraTargets()
        {
            List<TeamMember> allParticipants = teamManager.allParticipants;
            targets = new Transform[allParticipants.Count];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = allParticipants[i].gameObject.transform;
            }
        }

        private void Move()
        {
            FindAveragePosition();
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref moveVelocity,
                dampTime
            );
        }

        private void FindAveragePosition()
        {
            Vector3 averagePosition = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null || !targets[i].gameObject.activeSelf)
                {
                    continue;
                }

                averagePosition += targets[i].position;
                numTargets++;
            }

            if (numTargets > 0)
            {
                averagePosition /= numTargets;
            }

            averagePosition.y = transform.position.y;
            desiredPosition = averagePosition;
        }

        private void Zoom()
        {
            float requiredSize = FindRequiredSize();
            mainCamera.orthographicSize = Mathf.SmoothDamp(
                mainCamera.orthographicSize,
                requiredSize,
                ref zoomSpeed,
                dampTime
            );
        }

        private float FindRequiredSize()
        {
            float size = 0f;
            Vector3 desiredLocalPosition = transform.InverseTransformPoint(desiredPosition);

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null || !targets[i].gameObject.activeSelf)
                {
                    continue;
                }

                Vector3 targetLocalPosition = transform.InverseTransformPoint(targets[i].position);
                Vector3 desiredPositionToTarget = targetLocalPosition - desiredLocalPosition;
                size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.y));
                size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.x) / mainCamera.aspect);
            }

            size += screenEdgeBuffer;
            size = Mathf.Max(size, minSize);

            return size;
        }
    }
}
