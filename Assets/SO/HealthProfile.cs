using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthProfile", menuName = "Survival/Health Profile")]
public class HealthProfile : ScriptableObject
{
    [Header("Максимальные значения")]
    public float MaxHealth = 100f;
    public float MaxHunger = 100f;
    public float MaxThirst = 100f;
    public float MaxStamina = 100f;

    [Header("Скорость убывания в сек")]
    public float HungerDecay = 0.5f;
    public float ThirstDecay = 0.8f;
    public float StaminaRegen = 5f;

    [Header("Урон при истощении")]
    public float StarvationDamage = 2f;
}