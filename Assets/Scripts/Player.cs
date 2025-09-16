
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;



public class Player : LivingEntity
{

    private int level;

    public List<SpriteLibraryAsset> spriteAssets = new List<SpriteLibraryAsset>();
    private Animator animator;
    private AudioSource audioSource;
    private SpriteLibrary spriteLibrary;



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
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteLibrary = GetComponentInChildren<SpriteLibrary>();
    }
    public void Setup(int character_ID)
    {

        int idx = character_ID - 10101;
        spriteLibrary.spriteLibraryAsset = spriteAssets[idx];
        spriteLibrary.RefreshSpriteResolvers();

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
        
        if (target != null)
            target.OnDamage(AttackDamage);
    }
    public void UseSkill()
    {
       
        target.OnDamage(SkillDamage);
        target.AddStatus(SkillEffect.Effect_Type, 100, float.Parse(SkillData.Effect_1_Duration) / 1000);
        skillTimer = 0;
    }

    private void FindTarget()
    {
        if (battleManager.AliveEnemies.Count > 0)
        {
            target = battleManager.AliveEnemies[0];
        }
    }

    protected override void Die()
    {
        base.Die();
    }

}
