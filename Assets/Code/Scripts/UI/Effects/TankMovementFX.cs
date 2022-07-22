using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankMovement))]
    public class TankMovementFX : MonoBehaviour
    {
        public AudioSource movementAudio;
        public AudioClip engineIdling;
        public AudioClip engineDriving;
        public float pitchRange = 0.2f;

        private TankMovement movement;
        private float originalPitch;

        private void OnEnable()
        {
            movementAudio.Play();
            movementAudio.loop = true;
        }

        private void Start()
        {
            movement = GetComponent<TankMovement>();
            originalPitch = movementAudio.pitch;
        }

        private void LateUpdate()
        {
            EngineAudio();
        }

        private void OnDisable()
        {
            movementAudio.Stop();
        }

        private void EngineAudio()
        {
            if (Mathf.Abs(movement.movementInputValue) < 0.1f && Mathf.Abs(movement.turnInputValue) < 0.1f)
            {
                if (movementAudio.clip == engineDriving)
                {
                    movementAudio.clip = engineIdling;
                    movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                    movementAudio.Play();
                }
            }
            else
            {
                if (movementAudio.clip == engineIdling)
                {
                    movementAudio.clip = engineDriving;
                    movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                    movementAudio.Play();
                }
            }
        }
    }
}
