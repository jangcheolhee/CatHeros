using UnityEngine;


public enum Languages
{
    Korean,
    English,
    Japanese,
}

public static class DataTableIds
{
    public static readonly string[] StringTableIds =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp",
    };
    public static string String => StringTableIds[(int)Variables.Language];
    public static readonly string Character = "Character";
    public static readonly string Skill = "Skill";
    public static readonly string Stage = "Stage";
    public static readonly string Wave = "Wave";
    public static readonly string Monster = "Monster";
    public static readonly string Effect = "Effect";
}


public static class Variables
{
    public static Languages Language = Languages.Korean;
}

