
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;



public class Player : LivingEntity
{
    private readonly int isDie = Animator.StringToHash("IsDie");
    private readonly int isAttack = Animator.StringToHash("IsAttack");
    private readonly int isSkill = Animator.StringToHash("IsSkill");
    private int level;

  
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color originColor;



    public CharacterData characterData;
    public SkillData BasicAttack { get; private set; }
    public SkillData SkillData { get; private set; }
    public EffectData SkillEffect { get; private set; } = null;


    public int Max_HP
    {
        get
        {
            return characterData.Base_HP;
        }
    }
    public int AttackDamage
    {
        get
        {
            return (int)((characterData.Base_ATK + AddAttack) * BasicAttack.Power_Coeff_ATK );
        }
    }
    public int SkillDamage
    {
        get
        {
            return (int)((SkillData.Base_Power + (characterData.Base_ATK + AddAttack) * SkillData.Power_Coeff_ATK));
        }
    }
    public int Speed
    {
        get
        {
            return characterData.Base_SPD;
        }
    }
    public int Defence { get; private set; }

    public string Position
    {
        get
        {
            return characterData.Position;
        }
    }  // Tanker¿Œ¡ˆ   

    private LivingEntity target;

    private float attackTimer;
    private float skillTimer;
    private float AttackInterval
    {
        get
        {
            return BasicAttack.Base_SPD / (1 + Speed / BasicAttack.SPD_Factor);
        }
    }

    public BattleManager battleManager;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    public void Setup(int character_ID)
    {

        characterData = DataTableManger.CharacterTable.Get(character_ID);
        BasicAttack = DataTableManger.SkillTable.Get(characterData.Basic_attack_ID);
        SkillData = DataTableManger.SkillTable.Get(characterData.Skill_Set_ID);
        if (int.TryParse(SkillData.Effect_1_ID, out int id))
        {
            SkillEffect = DataTableManger.EffectTable.Get(id);
        }

        MaxHP = Max_HP;

        var health = GetComponent<PlayerHealth>();
        if (health != null) health.Refresh();
    }


    protected override void Update()
    {
        base.Update();
        if (IsDead) return;
        if (target == null || target.IsDead)
        {
            target = null;
            FindTarget();
        }
        attackTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;

        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0;
            Attack();
        }
        if(battleManager.IsAuto && skillTimer > SkillData.Cooldown)
        {
            UseSkill();
        }
    }
    private void Attack()
    {
        animator.SetTrigger(isAttack);
        if (target != null)
            target.OnDamage(AttackDamage);
        
    }
    public void UseSkill()
    {

        animator.SetTrigger(isSkill);
        target.OnDamage(SkillDamage);
        DamageTextSpawner.Instance.SpawnDamageText(SkillData.Skill_Name, transform.position + Vector3.up * 1.2f);

        if (SkillEffect != null)
        {
            
            target.AddStatus(SkillEffect.Effect_Type, 100, float.Parse(SkillData.Effect_1_Duration) / 1000);
        }
        skillTimer = 0;
    }
    

    private void FindTarget()
    {
        if (battleManager.AliveEnemies.Count > 0)
        {
            target = battleManager.AliveEnemies[0];
        }
    }
    public override void OnDamage(int damage)
    {
        StartCoroutine(CorDamage());
        base.OnDamage(damage);
    }
    protected override void Die()
    {
        animator.SetTrigger(isDie);
        base.Die();
    }
    private IEnumerator CorDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originColor; ;

    }

}
