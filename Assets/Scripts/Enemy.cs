using System.Collections;
using UnityEngine;

public class Enemy : LivingEntity
{
    private readonly int isDie = Animator.StringToHash("IsDie");
    private readonly int isAttack = Animator.StringToHash("IsAttack");
    private readonly int isSkill = Animator.StringToHash("IsSkill");
    private readonly int IsWalk = Animator.StringToHash("IsWalk");

    private Animator animator;
    public GameObject bulletPrefab;
    public MonsterData monsterData;
    public SkillData basicAttack { get; private set; }
    public SkillData skillData { get; private set; }
    private SpriteRenderer spriteRenderer;
    private Color originColor;
    private bool IsAttack = true;
    public enum Status
    {
        Idle,
        Trace,
        Back,
    }
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
    private float attackRange;
    private Vector2 InitPosition;
    private float speed = 3f;
    private float skillTimer;


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