using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankHealth))]
    public class TankHeatlhUI : MonoBehaviour
    {
        public Slider slider;
        public Image fillImage;
        public Color playerOneColor = Color.green;
        public Color playerTwoColor = Color.magenta;

        private TankHealth health;
        private TankPlayerController playerController;

        private void Start()
        {
            health = GetComponent<TankHealth>();
            playerController = GetComponent<TankPlayerController>();
        }

        private void OnEnable()
        {
            slider.gameObject.SetActive(true);
        }

        private void LateUpdate()
        {
            slider.value = health.currentHealth;

            if (playerController != null && playerController.enabled)
            {
                slider.gameObject.SetActive(true);

                if (playerController.playerControlsNumber == PlayerControlsNumber.FIRST)
                {
                    fillImage.color = playerOneColor;
                }

                if (playerController.playerControlsNumber == PlayerControlsNumber.SECOND)
                {
                    fillImage.color = playerTwoColor;
                }
            }
            else
            {
                slider.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            slider.gameObject.SetActive(false);
        }
    }
}
