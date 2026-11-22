using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthComponent health;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI thirstText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI statusText;

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnStatsChanged += UpdateUI;
            health.OnDied += HandleDeath;
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnStatsChanged -= UpdateUI;
            health.OnDied -= HandleDeath;
        }
    }

    private void UpdateUI()
    {
        if (healthText) healthText.text = $"HP: {health.Health:F0}";
        if (hungerText) hungerText.text = $"Food: {health.Hunger:F0}";
        if (thirstText) thirstText.text = $"Water: {health.Thirst:F0}";
        if (staminaText) staminaText.text = $"Stamina: {health.Stamina:F0}";
        if (statusText != null)
        {
            if (health.IsDead)
            {
                statusText.text = "Мертв";
                statusText.color = Color.red;
            }
            else
            {
                statusText.text = "Жив";
                statusText.color = Color.green;
            }
        }
    }

    private void HandleDeath()
    {
        UpdateUI();
    }

    public void AddHealth() => health?.IncreaseHealth(10f);
    public void RemoveHealth() => health?.DecreaseHealth(10f);

    public void AddHunger() => health?.IncreaseHunger(10f);
    public void RemoveHunger() => health?.DecreaseHunger(10f);

    public void AddThirst() => health?.IncreaseThirst(10f);
    public void RemoveThirst() => health?.DecreaseThirst(10f);

    public void AddStamina() => health?.IncreaseStamina(10f);
    public void RemoveStamina() => health?.DecreaseStamina(10f);
}