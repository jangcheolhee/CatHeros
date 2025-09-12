using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
public class CharacterData
{
    public string Character_ID { get; set; }
    public string Name { get; set; }
    public string Rarity {  get; set; }
    public string Position {  get; set; }
    public string Base_HP {  get; set; }
    public string Base_ATK {  get; set; }
    public string Base_DEF {  get; set; }
    public string Base_SPD {  get; set; }
    public string Skill_Set_ID {  get; set; }
    public string Basic_attack_ID {  get; set; }

}
public class CharacterTable : DataTable
{
    private readonly Dictionary<string, CharacterData> table = new Dictionary<string, CharacterData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<CharacterData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Character_ID))
            {
                table.Add(item.Character_ID, item);
            }
            else
            {
                //Debug.LogError("캐릭터 ID 아이디 중복!");
            }
            
        }
        
    }
    public CharacterData Get(string Character_ID)
    {
        
        if (!table.ContainsKey(Character_ID))
        {
            return null;
        }
        return table[Character_ID];
    }
}
