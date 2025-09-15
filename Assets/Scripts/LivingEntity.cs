using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHP { get; protected set; } = 1000;
    public float CurrentHP { get; protected set; }
    public bool IsDead { get; private set; }
    public event Action<float, float> OnHealthChanged; // (현재 HP, 최대 HP)

    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        CurrentHP = MaxHP;
        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }
    
    public virtual void OnDamage(int damage)
    {
        if (IsDead) return;

        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;
        DamageTextSpawner.Instance.SpawnDamageText(damage, transform.position + Vector3.up * 1.5f);

        OnHealthChanged?.Invoke(CurrentHP, MaxHP);

        if (CurrentHP <= 0 && !IsDead)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
