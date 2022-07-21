﻿using UnityEngine;

namespace Tanks.Gameplay
{
    public class CameraControl : MonoBehaviour
    {
        public float dampTime = 0.2f;
        public float screenEdgeBuffer = 4f;
        public float minSize = 6.5f;

        [HideInInspector] public Transform[] targets;

        private Camera mainCamera;
        private float zoomSpeed;
        private Vector3 moveVelocity;
        private Vector3 desiredPosition;

        private void Awake()
        {
            mainCamera = GetComponentInChildren<Camera>();
        }

        private void FixedUpdate()
        {
            Move();
            Zoom();
        }

        public void SetStartPositionAndSize()
        {
            FindAveragePosition();
            transform.position = desiredPosition;
            mainCamera.orthographicSize = FindRequiredSize();
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
                if (!targets[i].gameObject.activeSelf)
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
                if (!targets[i].gameObject.activeSelf)
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
