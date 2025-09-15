using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public string Skill_ID {  get; set; }
    public string Skill_Name { get; set; }
    public string Skill_Type {  get; set; }
    public string Cooldown {  get; set; }
}
public class SkillDataTable : DataTable
{
    private readonly Dictionary<string, SkillData> table = new Dictionary<string, SkillData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<SkillData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Skill_ID))
            {
                table.Add(item.Skill_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }

    }
    public SkillData Get(string id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
