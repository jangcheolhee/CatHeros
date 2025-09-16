
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;


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
    public EffectData SkillEffect { get; private set; }

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
            return (int)(characterData.Base_ATK * BasicAttack.Power_Coeff_ATK + AddAttack);
        }
    }
    public int SkillDamage
    {
        get
        {
            return (int)((SkillData.Base_Power + characterData.Base_ATK * SkillData.Power_Coeff_ATK));
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
    }  // Tanker인지   



    private Enemy target;

    private float attackTimer;
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

        SkillEffect = DataTableManger.EffectTable.Get(4404);
        Debug.Log("skell");



        MaxHP = Max_HP;
        OnDamage(0);

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


        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0;
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
        target.OnDamage(SkillDamage);


        target.AddStatus(1, 3, float.Parse(SkillData.Effect_1_Duration) / 1000);
        


        //Debug.Log($"스킬 사용");
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
        animator.SetTrigger("IsDie");
    }

}
