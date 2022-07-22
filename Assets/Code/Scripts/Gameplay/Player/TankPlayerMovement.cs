using UnityEngine;

namespace Tanks.Gameplay
{
    public class TankPlayerMovement : TankMovement
    {
        public float speed = 12f;
        public float turnSpeed = 180f;

        public float movementInputValue;
        public float turnInputValue;

        [HideInInspector] public Rigidbody rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            rigidBody.isKinematic = false;
            movementInputValue = 0f;
            turnInputValue = 0f;
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void OnDisable()
        {
            rigidBody.isKinematic = true;
        }

        private void Move()
        {
            Vector3 movementVector = transform.forward * movementInputValue * speed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + movementVector);
        }

        private void Turn()
        {
            float turn = turnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rigidBody.MoveRotation(rigidBody.rotation * turnRotation);
        }

        public override bool IsMoving()
        {
            if (Mathf.Abs(movementInputValue) >= 0.1f || Mathf.Abs(turnInputValue) >= 0.1f)
            {
                return true;
            }

            return false;
        }
    }
}
