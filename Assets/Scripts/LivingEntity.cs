using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum StatusEffect
{
    None = 0,
    Bleed = 1 << 0,
    Heal = 1 << 1,
    Stun = 1 << 2,
    Silence = 1 << 3,
    AttackUp = 1 << 4,
    AttackDown = 1 << 5,
    DefenseUp = 1 << 6,
    DefenseDown = 1 << 7,
    Burn = 1 << 8,

}
public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHP { get; protected set; } = 1000;
    public float CurrentHP { get; protected set; }
    public bool IsDead { get; private set; }

    public StatusEffect currentStatus = StatusEffect.None;
    private Dictionary<StatusEffect, float> statusTimers = new();

    private float atkMultiplier = 1f;
    private float defMultiplier = 1f;

    public event Action<float, float> OnHealthChanged; // (현재 HP, 최대 HP)
    public event Action OnDeath;



    protected virtual void Update()
    {
        // 상태 지속시간 체크
        var expired = new List<StatusEffect>();

        foreach (var kvp in statusTimers.ToList())
        {
            statusTimers[kvp.Key] -= Time.deltaTime;
            if (statusTimers[kvp.Key] <= 0) expired.Add(kvp.Key);
        }

        foreach (var eff in expired)
            RemoveStatus(eff);
    }

    public void AddStatus(int type, float duration = 0f)
    {
        StatusEffect effect = (StatusEffect)(1 << type);
       
        currentStatus |= effect;
        if (duration > 0)
            statusTimers[effect] = duration;
        
        switch (effect)
        {
            case StatusEffect.AttackUp:
                atkMultiplier = 1.5f;
                Debug.Log($"{effect} {duration}");
                break;
            case StatusEffect.DefenseUp:
                defMultiplier = 0.7f;
                break;
            case StatusEffect.Bleed:
                StartCoroutine(DoTCoroutine(5, 1f));
                break;
        }
    }
    public void RemoveStatus(StatusEffect effect)
    {
        
        currentStatus &= ~effect;
        statusTimers.Remove(effect);
        Debug.Log(effect);
    }

    public bool HasStatus(StatusEffect effect) => (currentStatus & effect) != 0;
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
    private IEnumerator DoTCoroutine(int damage, float interval)
    {
        while (HasStatus(StatusEffect.Bleed))
        {
            OnDamage(damage);
            yield return new WaitForSeconds(interval);
        }
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
