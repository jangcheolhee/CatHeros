
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string Character_ID;
    private int level;
    private Animator animator;
    private AudioSource audioSource;
    private ParticleSystem attackEffect;

    public int MaxHP;
    private int CurrentHP;
    public float attackInterval = 2;
    private float attackTimer = 0;

    // 버프 디버프 상태이상 
    private List<Skill> skills = new List<Skill>();
    private float[] skillTimer;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }
    public void SetUp()
    {
        
        Character_ID = "0";
        var characterData = DataTableManger.CharacterTable.Get(Character_ID);
        skills.Add(new Skill("기본", 3, 10));
        skills.Add(new Skill("스킬", 4, 15));

        skillTimer = new float[skills.Count];

    }
    private void Start()
    {
        
        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Overrides/{name}Override");
        SetUp();
        Debug.Log(12324);
    }

    // Update is called once per frame
    private void Update()
    {

        for (int i = 0; i < skills.Count; i++)
        {
            skillTimer[i] += Time.deltaTime; 
            if (skillTimer[i] > skills[i].cooldown)
            {
                UseSkill(skills[i]);
                skillTimer[i] = 0;
            }
        }

    }
    void Attack()
    {
        // 
    }
    void UseSkill(Skill skill)
    {
        Debug.Log($"{skill.skill_ID} {skill.cooldown}{skill.damage}");
    }
    
    
}
