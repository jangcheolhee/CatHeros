using UnityEngine;

public class Skill
{
    public string skill_ID;
    public float cooldown;
    public int damage;
    public Skill(string skill_ID, float cooldown, int damage)
    {
        this.skill_ID = skill_ID;
        this.cooldown = cooldown;
        this.damage = damage;
    }
}
