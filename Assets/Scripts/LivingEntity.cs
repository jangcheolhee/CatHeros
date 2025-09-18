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
    public bool IsStunned {  get; private set; }

    public StatusEffect currentStatus = StatusEffect.None;
    private Dictionary<StatusEffect, float> statusTimers = new();



    public event Action<float, float> OnHealthChanged; // (현재 HP, 최대 HP)
    public event Action OnDeath;


    protected int AddAttack { get; private set; }
    protected int AddDefense { get; private set; }

    protected virtual void Update()
    {

        var expired = new List<StatusEffect>();

        foreach (var kvp in statusTimers.ToList())
        {
            statusTimers[kvp.Key] -= Time.deltaTime;
            if (statusTimers[kvp.Key] <= 0) expired.Add(kvp.Key);
        }

        foreach (var eff in expired)
            RemoveStatus(eff);
    }

    public void AddStatus(int type, int amount, float duration = 0f)
    {
        StatusEffect effect = (StatusEffect)(1 << type);

        currentStatus |= effect;
        if (duration > 0)
            statusTimers[effect] = duration;

        switch (effect)
        {
            case StatusEffect.AttackUp:
                AddAttack += amount;
                break;
            case StatusEffect.DefenseUp:
                AddDefense += amount;
                break;
            case StatusEffect.Stun:
                Debug.Log("Stun");
                IsStunned = true;
                break;
            case StatusEffect.Bleed:
                StartCoroutine(DoTCoroutine(5, 1f, effect));
                break;
            case StatusEffect.Heal:
                Debug.Log($"Heal {duration}");
                StartCoroutine(HotCoroutine(500, 1f, effect));
       
                break;
        }
    }
    public void RemoveStatus(StatusEffect effect)
    {

        currentStatus &= ~effect;
        statusTimers.Remove(effect);
        switch (effect)
        {
            case StatusEffect.AttackUp:
                AddAttack = 0;
                break;
            case StatusEffect.DefenseUp:
                AddDefense = 0;
                break;
            case StatusEffect.Stun:
                IsStunned = true;
                break;
                
                
        }
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
        DamageTextSpawner.Instance.SpawnDamageText(damage.ToString(), transform.position + Vector3.up);

        OnHealthChanged?.Invoke(CurrentHP, MaxHP);

        if (CurrentHP <= 0 && !IsDead)
        {
            Die();
        }
    }
    private IEnumerator DoTCoroutine(int damage, float interval, StatusEffect effect)
    {
        while (HasStatus(effect))
        {
            OnDamage(damage);

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator HotCoroutine(int healAmount, float interval, StatusEffect effect)
    {
        while (HasStatus(effect))
        {
            Heal(healAmount);
            DamageTextSpawner.Instance.SpawnDamageText("Heal", transform.position + Vector3.up);
            yield return new WaitForSeconds(interval);
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        CurrentHP += amount;
        if (CurrentHP > MaxHP) CurrentHP = MaxHP;

        OnHealthChanged?.Invoke(CurrentHP, MaxHP);
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
