
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public AudioClip attackClip;

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
    private CharacterInfo character; 


    public int Max_HP
    {
        get
        {
            return character.Hp;
        }
    }
    public int AttackDamage
    {
        get
        {
            return (int)(AttackD  * BasicAttack.Power_Coeff_ATK);
        }
    }
    public int SkillDamage
    {
        get
        {
            return (int)((SkillData.Base_Power + AttackD * SkillData.Power_Coeff_ATK));
        }
    }
    public int Speed
    {
        get
        {
            return character.Spd;

        }
    }
    public int Defence
    {
        get
        {
           return character.Def + AddDefense;

        }
    }

    public int AttackD
    {
        get
        {
            return character.Atk + AddAttack;

        }
    }
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
        character = GameManager.Instance.saveCharacterList
           .FirstOrDefault(c => c.Character_ID.Character_ID == characterData.Character_ID);

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

            target.OnDamage(AttackDamage);
        }
        SkillSfxManager.Instance.PlaySfx(attackClip);
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
        Destroy(effect, 0.5f);
        var sound = Resources.Load<AudioClip>($"Audio/{SkillData.Skill_ID}");
        SkillSfxManager.Instance.PlaySfx(sound);

        if (SkillEffect != null)
        {
            switch(SkillEffect.Effect_ID)
            {
                case 4404:
                    skillTarget.AddStatus(SkillEffect.Effect_Type, (int)(MaxHP * (int.Parse(SkillData.Effect_1_Value) * 0.01)), float.Parse(SkillData.Effect_1_Duration) / 1000);
                    break;
                case 5402:
                    
                    skillTarget.AddStatus(SkillEffect.Effect_Type, 0, float.Parse(SkillData.Effect_1_Duration) / 1000);
                    
                    break;
                case 2410:
                    Debug.Log($"{Defence} {AddDefense}");
                    skillTarget.AddStatus(SkillEffect.Effect_Type, (int)(Defence * (int.Parse(SkillData.Effect_1_Value) * 0.01)), float.Parse(SkillData.Effect_1_Duration) / 1000);
                    Debug.Log($"{Defence} {AddDefense}");
                    break;
                case 1403:
                    skillTarget.AddStatus(SkillEffect.Effect_Type, (int)(AttackD  * (int.Parse(SkillData.Effect_1_Value) * 0.01)), float.Parse(SkillData.Effect_1_Duration) / 1000);

                    break;

            }

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
            Debug.Log(skillTimer);
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
        FormationRow frontRow = FormationRow.Front;
        FormationRow backRow = FormationRow.Rear;

        if (battleManager.AliveEnemies.ContainsKey(frontRow))
        {
            foreach (var enemy in battleManager.AliveEnemies[frontRow])
            {
                if (!enemy.IsDead)
                {
                    target = enemy;
                    return;
                }
            }
        }
        if (battleManager.AliveEnemies.ContainsKey(backRow))
        {
            foreach (var enemy in battleManager.AliveEnemies[backRow])
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
        FormationRow frontRow = FormationRow.Front;
        FormationRow backRow = FormationRow.Rear;
        switch (SkillData.Skill_ID)
        {
            case 32301:
            case 31304:
            case 32331:
            case 31335:
                if (battleManager.AliveEnemies.ContainsKey(frontRow))
                {
                    foreach (var enemy in battleManager.AliveEnemies[frontRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                if (battleManager.AliveEnemies.ContainsKey(backRow))
                {
                    foreach (var enemy in battleManager.AliveEnemies[backRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                break;
            case 31302:
            case 31330:
                skillTarget = this;
                break;
            case 32303:
            case 31328:
            case 31329:
            case 31334:
                if (battleManager.AliveEnemies.ContainsKey(backRow))
                {
                    foreach (var enemy in battleManager.AliveEnemies[backRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                if (battleManager.AliveEnemies.ContainsKey(frontRow))
                {
                    foreach (var enemy in battleManager.AliveEnemies[frontRow])
                    {
                        if (!enemy.IsDead)
                        {
                            skillTarget = enemy;
                            return;
                        }
                    }
                }
                break;
            case 32305:
            case 33332:
                float minHp = 1000000f;
                if (battleManager.Players.ContainsKey(frontRow))
                {
                    foreach (var player in battleManager.Players[frontRow])
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
                if (battleManager.Players.ContainsKey(backRow))
                {
                    foreach (var player in battleManager.Players[backRow])
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
        damage = Math.Clamp(damage - (int)(Defence * 0.15) , 0, damage);
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
