using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public int Skill_ID {  get; set; }
    public string Skill_Name { get; set; }
    public int Skill_Type {  get; set; }
    public int Base_Power {  get; set; }
    public float Power_Coeff_ATK {  get; set; }
    public int Range {  get; set; }
    public string Area {  get; set; }
    public int Cooldown {  get; set; }
    public int Base_SPD {  get; set; }
    public int SPD_Factor {  get; set; }
    public string Skill_Target {  get; set; }
    public string Effect_1_ID { get; set; }
    public string Effect_1_Value {  get; set; }
    public string Effect_1_Duration { get; set; }

}
public class SkillDataTable : DataTable
{
    private readonly Dictionary<int, SkillData> table = new Dictionary<int, SkillData>();
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
    public SkillData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
