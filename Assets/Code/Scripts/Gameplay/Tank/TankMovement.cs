using UnityEngine;

namespace Tanks.Gameplay
{
    public class TankMovement : MonoBehaviour
    {
        public int playerNumber = 1;
        public float speed = 12f;
        public float turnSpeed = 180f;

        public float movementInputValue { get; private set; }
        public float turnInputValue { get; private set; }

        private string movementAxisName;
        private string turnAxisName;
        private Rigidbody thisRigidbody;

        private void Awake()
        {
            thisRigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            thisRigidbody.isKinematic = false;
            movementInputValue = 0f;
            turnInputValue = 0f;
        }

        private void OnDisable()
        {
            thisRigidbody.isKinematic = true;
        }

        private void Start()
        {
            movementAxisName = GameConstants.AXIS_NAME_PLAYER_VERTICAL + playerNumber;
            turnAxisName = GameConstants.AXIS_NAME_PLAYER_HORIZONTAL + playerNumber;
        }

        private void Update()
        {
            movementInputValue = Input.GetAxis(movementAxisName);
            turnInputValue = Input.GetAxis(turnAxisName);
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void Move()
        {
            Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
            thisRigidbody.MovePosition(thisRigidbody.position + movement);
        }

        private void Turn()
        {
            float turn = turnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            thisRigidbody.MoveRotation(thisRigidbody.rotation * turnRotation);
        }
    }
}
