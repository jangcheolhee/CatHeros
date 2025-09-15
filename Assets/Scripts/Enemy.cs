using UnityEngine;

public class Enemy : LivingEntity
{
    public string enemyName;
    public int Damage = 20;

    
    private Player target;
    private float attackTimer;

    public BattleManager battleManager;

    private void Awake()
    {
       
    }

    private void Update()
    {
        if (IsDead) return;

        if (target == null)
            FindTarget();

        attackTimer += Time.deltaTime;
        if (target && attackTimer > 1.5f)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    private void Attack()
    {
       
        if (target != null)
            target.OnDamage(Damage);
    }

    private void FindTarget()
    {
        if (battleManager.Players.Count > 0)
        {
            target = battleManager.Players[0]; // 가장 앞의 플레이어를 기본 타겟
        }
    }

    protected override void Die()
    {
        base.Die();
        
        Debug.Log($"{enemyName} 사망");
    }
}