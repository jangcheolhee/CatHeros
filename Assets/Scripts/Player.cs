
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : MonoBehaviour
{
    
    private int level;
    private string cName;
    private Animator animator;
    private AudioSource audioSource;
    //private ParticleSystem attackEffect;
    private SpriteLibrary spriteLibrary;
    public List<SpriteLibraryAsset> spriteAssets = new List<SpriteLibraryAsset>();
    
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
        spriteLibrary = GetComponent<SpriteLibrary>();

    }
    public void Setup(string Character_ID)
    {
        
        int idx = int.Parse(Character_ID) - 10101;
        
        var characterData = DataTableManger.CharacterTable.Get(Character_ID);
        
        var basicAttack = DataTableManger.SkillTable.Get(characterData.Basic_attack_ID);
        var skill = DataTableManger.SkillTable.Get(characterData.Skill_Set_ID);
        skills.Add(new Skill(basicAttack.Skill_Name, 3, 10));
        skills.Add(new Skill(skill.Skill_Name, 4, 15));
        cName = characterData.Name;
       
        spriteLibrary.spriteLibraryAsset = spriteAssets[idx];
        spriteLibrary.RefreshSpriteResolvers();
        skillTimer = new float[skills.Count];

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
        animator.SetBool("IsSkill", true);
        StartCoroutine(Skill());
        Debug.Log($"{skill.skill_ID} {skill.cooldown}{skill.damage}");
    }
    
    private IEnumerator Skill()
    {
        Debug.Log(1);
        
        
        yield return new WaitForSeconds(0.5f);
        Debug.Log(2);
        //animator.SetBool("IsSkill", false);
    }
    
}
