using UnityEditor.U2D.Animation;
using UnityEngine;

public class Enemy : LivingEntity
{
    public MonsterData monsterData;
    public SkillData basicAttack { get; private set; }
    public SkillData skillData { get; private set; }

    public int AttackDamage
    {
        get
        {
            return (int)(monsterData.M_Base_ATK * basicAttack.Power_Coeff_ATK);
        }
    }
    public int SkillDamage
    {
        get
        {
            return (int)(skillData.Base_Power + monsterData.M_Base_ATK * skillData.Power_Coeff_ATK);
        }
    }
    public int Speed
    {
        get
        {
            return monsterData.M_Base_SPD;
        }
    }
    public int Defence { get; private set; }

    public string Position
    {
        get
        {
            return monsterData.M_Position;
        }
    }  // Tanker인지   
    
    private float attackTimer;
    private float AttackInterval
    {
        get
        {

            return Speed / (1 + basicAttack.Base_SPD / basicAttack.SPD_Factor);
        }
    }

    private Player target;
    private LivingEntity skillTarget;
    public BattleManager battleManager;

    private void Awake()
    {
       
    }
    public void Setup(int monster_ID)
    {
        monsterData = DataTableManger.MonsterTable.Get(monster_ID);
        basicAttack = DataTableManger.SkillTable.Get(monsterData.M_Basic_attack_ID);
        skillData = DataTableManger.SkillTable.Get(monsterData.M_Skill_Set_ID);

    }

    private void Update()
    {
        if (IsDead) return;

        if (target == null)
            FindTarget();

        attackTimer += Time.deltaTime;
        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    private void Attack()
    {
        if (target != null)
            target.OnDamage(AttackDamage);
    }
    public void UseSkill()
    {
        //animator.SetBool("IsSkill", true);
        //target.OnDamage(skill.damage);

        Debug.Log($"스킬 사용");
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
        
        
    }
}