using System.Collections;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Enemy : LivingEntity
{
    private Animator animator;

    public MonsterData monsterData;
    public SkillData basicAttack { get; private set; }
    public SkillData skillData { get; private set; }
    private SpriteRenderer spriteRenderer;
    private Color originColor;

    public int Max_HP
    {
        get
        {
            return monsterData.M_Base_HP;
        }
    }
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

            return basicAttack.Base_SPD / (1 + Speed / basicAttack.SPD_Factor);
        }
    }

    private Player target;
    private LivingEntity skillTarget;
    public BattleManager battleManager;

    private void Awake()
    {
       animator=GetComponent<Animator>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    public void Setup(int monster_ID)
    {
        monsterData = DataTableManger.MonsterTable.Get(monster_ID);
        basicAttack = DataTableManger.SkillTable.Get(monsterData.M_Basic_attack_ID);
        skillData = DataTableManger.SkillTable.Get(monsterData.M_Skill_Set_ID);

        MaxHP = Max_HP;

        var health = GetComponent<EnemyHealth>();
        if (health != null) health.Refresh();

    }

    protected override void Update()
    {
        base.Update();
        if (IsDead) return;

        if (target == null || target.IsDead)
            FindTarget();
        if(!IsStunned)
        {
            attackTimer += Time.deltaTime;
            
        }
        
        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0f;
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("IsAttack");
        if (target != null)
            target.OnDamage(AttackDamage);
    }
    public void UseSkill()
    {
        //animator.SetBool("IsSkill", true);
        //target.OnDamage(skill.damage);

        Debug.Log($"스킬 사용");
    }
    public override void OnDamage(int damage)
    {
        StartCoroutine(CorDamage());
        base.OnDamage(damage);
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
        animator.SetTrigger("IsDie");
        
    }
    private IEnumerator CorDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originColor; ;

    }
}