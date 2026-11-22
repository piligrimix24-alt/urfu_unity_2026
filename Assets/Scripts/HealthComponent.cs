using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private HealthProfile profile;

    private float _health;
    private float _hunger;
    private float _thirst;
    private float _stamina;

    public event Action OnStatsChanged;
    public event Action OnDied;

    public bool IsDead => _health <= 0f;

    public float Health => _health;
    public float Hunger => _hunger;
    public float Thirst => _thirst;
    public float Stamina => _stamina;

    private void Awake()
    {
        if (profile == null) profile = ScriptableObject.CreateInstance<HealthProfile>();

        _health = profile.MaxHealth;
        _hunger = profile.MaxHunger;
        _thirst = profile.MaxThirst;
        _stamina = profile.MaxStamina;
    }

    private void Update()
    {
        if (IsDead) return;

        float dt = Time.deltaTime;

        DecreaseHunger(profile.HungerDecay * dt);
        DecreaseThirst(profile.ThirstDecay * dt);
        IncreaseStamina(profile.StaminaRegen * dt);

        if (_hunger <= 0f || _thirst <= 0f)
        {
            DecreaseHealth(profile.StarvationDamage * dt);
        }
    }
    public void IncreaseHealth(float amount) => ChangeStat(ref _health, amount, profile.MaxHealth);
    public void DecreaseHealth(float amount) => ChangeStat(ref _health, -amount, profile.MaxHealth);

    public void IncreaseHunger(float amount) => ChangeStat(ref _hunger, amount, profile.MaxHunger);
    public void DecreaseHunger(float amount) => ChangeStat(ref _hunger, -amount, profile.MaxHunger);

    public void IncreaseThirst(float amount) => ChangeStat(ref _thirst, amount, profile.MaxThirst);
    public void DecreaseThirst(float amount) => ChangeStat(ref _thirst, -amount, profile.MaxThirst);

    public void IncreaseStamina(float amount) => ChangeStat(ref _stamina, amount, profile.MaxStamina);
    public void DecreaseStamina(float amount) => ChangeStat(ref _stamina, -amount, profile.MaxStamina);

    private void ChangeStat(ref float stat, float delta, float max)
    {
        if (IsDead && delta < 0) return;

        stat = Mathf.Clamp(stat + delta, 0f, max);
        OnStatsChanged?.Invoke();

        if (_health <= 0f && !IsDead)
        {
            _health = 0f;
            OnDied?.Invoke();
        }
    }
}