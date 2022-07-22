using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankShooting))]
    public class TankShootingUI : MonoBehaviour
    {
        public Slider aimSlider;

        private TankShooting shooting;

        private void OnEnable()
        {
            aimSlider.gameObject.SetActive(true);
        }

        private void Start()
        {
            shooting = GetComponent<TankShooting>();
        }

        private void LateUpdate()
        {
            aimSlider.value = shooting.currentLaunchForce;
        }

        private void OnDisable()
        {
            aimSlider.gameObject.SetActive(false);
        }
    }
}
