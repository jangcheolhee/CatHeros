using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public int Monster_ID {  get; set; }
    public string M_Name {  get; set; }
    public string M_Position {  get; set; }
    public int M_Rarity {  get; set; }
    public bool M_Boss_Flag {  get; set; }
    public int M_Base_HP {  get; set; }
    public int M_Base_ATK {  get; set; }
    public int M_Base_DEF {  get; set; }
    public int M_Base_SPD { get; set; }
    public int M_Skill_Set_ID {  get; set; }
    public int M_Basic_attack_ID {  get; set; }

}
public class MonsterTable : DataTable
{
    private readonly Dictionary<int, MonsterData> table = new Dictionary<int, MonsterData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<MonsterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Monster_ID))
            {
                table.Add(item.Monster_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }

    }
    public MonsterData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
