
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
    public SkillData basicAttack { get; private set; }
    public SkillData skillData { get; private set; }

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
            return (int)(characterData.Base_ATK * basicAttack.Power_Coeff_ATK);
        }
    }
    public int SkillDamage
    {
        get
        {
            return (int)(skillData.Base_Power + characterData.Base_ATK * skillData.Power_Coeff_ATK);
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
    private LivingEntity skillTarget;
    private float attackTimer;
    private float AttackInterval
    {
        get
        {
           
            return basicAttack.Base_SPD / (1 + Speed / basicAttack.SPD_Factor);
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
        characterData = DataTableManger.CharacterTable.Get(character_ID);
        basicAttack = DataTableManger.SkillTable.Get(characterData.Basic_attack_ID);
        skillData = DataTableManger.SkillTable.Get(characterData.Skill_Set_ID);
        spriteLibrary.spriteLibraryAsset = spriteAssets[idx];
        spriteLibrary.RefreshSpriteResolvers();
        Debug.Log(AttackInterval);
        MaxHP = Max_HP;
        OnDamage(0);

    }
    

    // Update is called once per frame
    private void Update()
    {
        if (IsDead) return;
        if (target == null || target.IsDead)
        {
            target = null;
            FindTarget();
        }
        attackTimer += Time.deltaTime;
        //skillTimer += Time.deltaTime;

        if (target && attackTimer > AttackInterval) // <- 수정 필요
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

        Debug.Log($"스킬 사용");
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
