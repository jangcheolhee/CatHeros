using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LevelUpData
{
    public int Growth_Curve_ID { get; set; }
    public string Rarity { get; set; }
    public string Position { get; set; }
    public int HP { get; set; }
    public int ATK { get; set; }
    public int DEF { get; set; }
    public float SPD { get; set; }


}
public class LevelUpTable : DataTable
{
    private readonly Dictionary<int, LevelUpData> table = new Dictionary<int, LevelUpData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<LevelUpData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Growth_Curve_ID))
            {
                table.Add(item.Growth_Curve_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }
    }
    public LevelUpData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
