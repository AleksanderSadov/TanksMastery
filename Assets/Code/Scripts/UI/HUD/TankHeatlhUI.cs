using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class TankHeatlhUI : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;

    private TankHealth health;

    private void Start()
    {
        health = GetComponent<TankHealth>();
    }

    private void OnEnable()
    {
        slider.gameObject.SetActive(true);
    }

    private void Update()
    {
        SetHealthUI();
    }

    private void OnDisable()
    {
        slider.gameObject.SetActive(false);
    }

    private void SetHealthUI()
    {
        slider.value = health.currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, health.currentHealth / health.startingHealth);
    }
}
