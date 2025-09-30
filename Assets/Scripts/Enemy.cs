using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : LivingEntity
{
    private readonly int isDie = Animator.StringToHash("IsDie");
    private readonly int isAttack = Animator.StringToHash("IsAttack");
    private readonly int isSkill = Animator.StringToHash("IsSkill");
    private readonly int IsWalk = Animator.StringToHash("IsWalk");

    private Animator animator;
    public GameObject bulletPrefab;
    public MonsterData monsterData;
    GameObject effectPrefab;
    public SkillData basicAttack { get; private set; }
    public SkillData skillData { get; private set; }
    public EffectData SkillEffect { get; private set; } = null;

    private SpriteRenderer spriteRenderer;
    private Color originColor;
    private bool IsAttack = true;
 
    private Status currentStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;
            switch (CurrentStatus)
            {
                case Status.Idle:
                    animator.SetBool(IsWalk, false);
                    IsAttack = true;

                    break;
                case Status.Trace:
                    animator.SetBool(IsWalk, true);

                    break;
            }
        }
    }
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
    public int Defence
    {
        get
        {
            return monsterData.M_Base_DEF;
        }
    }

    public string Position
    {
        get
        {
            return monsterData.M_Position;
        }
    }  // Tanker¿Œ¡ˆ   
    
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
    private float attackRange;
    private Vector2 InitPosition;
    private float speed = 3f;
    private float skillTimer;
    private float SkillInterval
    {
        get
        {

            return skillData.Base_SPD / (1 + Speed / skillData.SPD_Factor);
        }
    }

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
        AnimatorOverrideController overrideCtrl =
            Resources.Load<AnimatorOverrideController>($"Overrides/{monster_ID}");
        
        animator.runtimeAnimatorController = overrideCtrl;
        MaxHP = Max_HP;

        var health = GetComponent<EnemyHealth>();
        if (health != null) health.Refresh();
        InitPosition = transform.position;
        switch (monsterData.M_Basic_attack_ID)
        {
            case 11306:
                attackRange = 1f;
                break;
            case 11307:
                attackRange = 2f;
                break;
            case 11308:
                attackRange = 4f;
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead) return;

        if (target == null || target.IsDead)
            FindTarget();
        skillTimer += Time.deltaTime;
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;

            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Back:
                UpdateBack();
                break;

        }

        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0f;
            Attack();
        }
        if (skillTarget && skillTimer > SkillInterval)
        {
            UseSkill();
            skillTimer = 0f;
        }

    }
    private void UpdateBack()
    {
        transform.position = Vector3.Lerp(
        transform.position,
        InitPosition,
        speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, InitPosition) < 0.01f)
        {
            CurrentStatus = Status.Idle;
        }
    }
    private void UpdateTrace()
    {
        if (IsAttack)
        {
            transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            {
                animator.SetTrigger(isAttack);
                IsAttack = false;
                Shoot();
                StartCoroutine(Attack());
            }
        }

    }

    private IEnumerator Attack()
    {
        
        if (target != null)
        {
            if (monsterData.M_Basic_attack_ID == 11308)
                target.OnDamage(-AttackDamage);
            else
                target.OnDamage(AttackDamage);
        }
        yield return new WaitForSeconds(0.5f);
        CurrentStatus = Status.Back;



    }

    private void UpdateIdle()
    {
        if (!IsStunned)
        {
            attackTimer += Time.deltaTime;

        }
        attackTimer += Time.deltaTime;
        skillTimer += Time.deltaTime;

        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0;
            
            CurrentStatus = Status.Trace;
        }
       
    }
    public void Shoot()
    {
        if (target != null)
        {

            GameObject proj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Bullet>().Init(target);
        }
    }
    public void UseSkill()
    {
        FindSkillTarget();

        animator.SetTrigger(isSkill);
        skillTarget.OnDamage(SkillDamage);
        var effect = Instantiate(effectPrefab, skillTarget.transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        var sound = Resources.Load<AudioClip>($"Audio/{skillData.Skill_ID}");
        SkillSfxManager.Instance.PlaySfx(sound);

        if (SkillEffect != null)
        {
            //skillTarget.AddStatus(SkillEffect.Effect_Type, 100, 1);

            skillTarget.AddStatus(SkillEffect.Effect_Type, 100, float.Parse(skillData.Effect_1_Duration) / 1000);
        }
        skillTimer = 0;
    }

    private void FindSkillTarget()
    {
        skillTarget = null;
        FormationRow frontRow = FormationRow.Front;
        FormationRow backRow = FormationRow.Rear;
        switch (skillData.Skill_ID)
        {
            
            case 31310:
            case 31314:
            case 32315:
            case 33316:
            case 32320:
            case 32321:
                if (battleManager.Players.ContainsKey(frontRow))
                {
                    foreach (var enemy in battleManager.Players[frontRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                if (battleManager.Players.ContainsKey(backRow))
                {
                    foreach (var enemy in battleManager.Players[backRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                break;
            case 31309:
            case 31319:
                skillTarget = this;
                break;
            case 31311:
            case 34313:
            case 31317:
            case 31318:
                if (battleManager.Players.ContainsKey(backRow))
                {
                    foreach (var enemy in battleManager.Players[backRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                if (battleManager.Players.ContainsKey(frontRow))
                {
                    foreach (var enemy in battleManager.Players[frontRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                break;
            
            case 32312:
            case 31323:
            case 33324:
                float minHp = 1000000f;
                if (battleManager.AliveEnemies.ContainsKey(frontRow))
                {
                    foreach (var player in battleManager.AliveEnemies[frontRow])
                    {
                        if (!player.IsDead)
                        {
                            if (player.CurrentHP < minHp)
                            {
                                skillTarget = player;
                                minHp = player.CurrentHP;
                            }
                        }
                    }
                }
                if (battleManager.AliveEnemies.ContainsKey(backRow))
                {
                    foreach (var player in battleManager.AliveEnemies[backRow])
                    {
                        if (player.CurrentHP < minHp)
                        {
                            skillTarget = player;
                            minHp = player.CurrentHP;
                        }
                    }
                }

                return;
                break;
        }
    }

    public override void OnDamage(int damage)
    {
        StartCoroutine(CorDamage());
        damage = Math.Clamp(damage - (int)(Defence * 0.1), 0, damage);
        base.OnDamage(damage);
    }

    private void FindTarget()
    {
        FormationRow priorityRow = FormationRow.Front;
        FormationRow backupRow = FormationRow.Rear;


        if (battleManager.Players.ContainsKey(priorityRow))
        {
            foreach (var player in battleManager.Players[priorityRow])
            {
                if (player != null && !player.IsDead)
                {
                    target = player;
                    return;
                }
            }
        }


        if (battleManager.Players.ContainsKey(backupRow))
        {
            foreach (var player in battleManager.Players[backupRow])
            {
                if (player != null && !player.IsDead)
                {
                    target = player;
                    return;
                }
            }
        }


        target = null;
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