
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Player : LivingEntity
{
    


    private int level;
    
   
    public List<SpriteLibraryAsset> spriteAssets = new List<SpriteLibraryAsset>();

    public int Damage { get; private set; } = 50;
    public string Position {  get; private set; }  // Tanker인지   
    public SkillData BasicAttack {  get; private set; }
    public SkillData Skill { get; private set; }

    public string characterName;
    

    private Animator animator;
    private AudioSource audioSource;
    private SpriteLibrary spriteLibrary;

    private Enemy target;
    private float attackTimer;

    public BattleManager battleManager;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteLibrary = GetComponentInChildren<SpriteLibrary>();
    }
    public void Setup(int Character_ID)
    {
        
        int idx = Character_ID - 10101;
        var characterData = DataTableManger.CharacterTable.Get(Character_ID);
        Damage = characterData.Base_ATK;
        BasicAttack = DataTableManger.SkillTable.Get(characterData.Basic_attack_ID);
        Skill = DataTableManger.SkillTable.Get(characterData.Skill_Set_ID);
        spriteLibrary.spriteLibraryAsset = spriteAssets[idx];
        spriteLibrary.RefreshSpriteResolvers();

    }

    // Update is called once per frame
    private void Update()
    {
        if(IsDead) return;
        if (target == null || target.IsDead)
        {
            target = null;
            FindTarget();
        }
        attackTimer += Time.deltaTime;
        //skillTimer += Time.deltaTime;

        if(target && attackTimer > BasicAttack.Cooldown + 1) // <- 수정 필요
        {
            attackTimer = 0;
            Attack();
        }
    }
    private void Attack()
    {
        animator.SetTrigger("IsAttack");
        if (target != null)
            target.OnDamage(Damage);
    }
    public void UseSkill()
    {
        //animator.SetBool("IsSkill", true);
        //target.OnDamage(skill.damage);
        
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
