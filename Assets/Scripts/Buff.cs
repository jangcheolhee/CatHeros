
using UnityEngine;
public enum BuffTypes
{
    None = 0,
    Buff,
    Debuff,
    Heal,

}

public class Buff
{
    float duration;
    BuffTypes type;
    bool stackable;

}
