
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;


public class Player : LivingEntity
{
    private readonly int isDie = Animator.StringToHash("IsDie");
    private readonly int isAttack = Animator.StringToHash("IsAttack");
    private readonly int isSkill = Animator.StringToHash("IsSkill");
    private readonly int IsWalk = Animator.StringToHash("IsWalk");
    private int level;
    private bool IsAttack = true;
    public GameObject bulletPrefab;
    public enum Status
    {
        Idle,
        Trace,
        Back,
    }
    GameObject effectPrefab;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Color originColor;
    private Vector3 InitPosition;
    private float attackRange;
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
            return (int)((characterData.Base_ATK + AddAttack) * BasicAttack.Power_Coeff_ATK);
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
    private float speed = 3f;
    private LivingEntity target;
    private LivingEntity skillTarget;

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
        AnimatorOverrideController overrideCtrl =
            Resources.Load<AnimatorOverrideController>($"Overrides/{character_ID}");
        animator.runtimeAnimatorController = overrideCtrl;
        if (int.TryParse(SkillData.Effect_1_ID, out int id))
        {
            SkillEffect = DataTableManger.EffectTable.Get(id);
        }

        MaxHP = Max_HP;

        var health = GetComponent<PlayerHealth>();
        if (health != null) health.Refresh();
        effectPrefab = Resources.Load<GameObject>($"Effects/{characterData.Skill_Set_ID}");
        InitPosition = transform.position;
        switch (characterData.Basic_attack_ID)
        {
            case 11306:
                attackRange = 0.5f;
                break;
            case 11307:
                attackRange = 1.5f;
                break;
            case 11308:
                attackRange = 2f;
                break;
        }
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

    }

    private void UpdateBack()
    {
        transform.position = Vector3.Lerp(
        transform.position,
        InitPosition,
        speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, InitPosition) < 0.2f)
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
                StartCoroutine(Attack());
                if (attackRange > 1) Shoot();
            }
        }

    }

    private IEnumerator Attack()
    {
        
        if (target != null)
        {
            if (characterData.Basic_attack_ID == 11308)
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
        if (target && battleManager.IsAuto && skillTimer > SkillData.Cooldown)
        {
            AutoUseSkill();
        }
        if (target && attackTimer > AttackInterval)
        {
            attackTimer = 0;
            //Attack();
            CurrentStatus = Status.Trace;
        }
        
    }


    public void UseSkill()
    {

        FindSkillTarget();

        animator.SetTrigger(isSkill);
        skillTarget.OnDamage(SkillDamage);
        var effect = Instantiate(effectPrefab, skillTarget.transform.position, Quaternion.identity);
        Destroy(effect, 1);
        if (SkillEffect != null)
        {
            skillTarget.AddStatus(SkillEffect.Effect_Type, 100, 1);

            //skillTarget.AddStatus(SkillEffect.Effect_Type, 100, float.Parse(SkillData.Effect_1_Duration) / 1000);
        }
        skillTimer = 0;


    }
    public void AutoUseSkill()
    {

        int idx = -1;

        idx = battleManager.Players[FormationRow.Front].IndexOf(this);
        if (idx == -1)
        {
            idx = battleManager.Players[FormationRow.Rear].IndexOf(this) + battleManager.Players[FormationRow.Front].Count;
        }

        if (idx >= 0 && idx < battleManager.battleUIManager.skillButtons.Count)
        {
            var btn = battleManager.battleUIManager.skillButtons[idx];
            btn.button.onClick.Invoke();
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

    private void FindTarget()
    {
        FormationRow priorityRow = FormationRow.Front;
        FormationRow backupRow = FormationRow.Rear;
        if (characterData.Basic_attack_ID == 11308)
        {
            float minHp = 1000000f;
            if (battleManager.Players.ContainsKey(priorityRow))
            {

                foreach (var player in battleManager.AliveEnemies[priorityRow])
                {
                    if (!player.IsDead)
                    {
                        if (player.CurrentHP < minHp)
                        {
                            target = player;
                            minHp = player.CurrentHP;
                        }


                    }
                }
            }
            if (battleManager.Players.ContainsKey(backupRow))
            {
                foreach (var player in battleManager.AliveEnemies[backupRow])
                {
                    if (player.CurrentHP < minHp)
                    {
                        target = player;
                        minHp = player.CurrentHP;
                    }
                }
            }
            return;

        }
        if (battleManager.AliveEnemies.ContainsKey(priorityRow))
        {
            foreach (var enemy in battleManager.AliveEnemies[priorityRow])
            {
                if (!enemy.IsDead)
                {
                    target = enemy;
                    return;
                }
            }
        }
        if (battleManager.AliveEnemies.ContainsKey(backupRow))
        {
            foreach (var enemy in battleManager.AliveEnemies[backupRow])
            {
                if (!enemy.IsDead)
                {
                    target = enemy;
                    return;
                }
            }
        }
        target = null;
    }
    private void FindSkillTarget()
    {
        skillTarget = null;
        if (SkillData.Effect_1_Target == "1")
        {

            FindTarget();
            skillTarget = target;
            return;

        }
        skillTarget = target;
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
