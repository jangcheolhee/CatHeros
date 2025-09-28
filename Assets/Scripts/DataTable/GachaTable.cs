using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GachaData
{
    public int Gacha_ID { get; set; }
    public int Gacha_Type { get; set; }
    public int Result_ID { get; set; }
    public int Amount { get; set; }
    public int Rarity { get; set; }
    public string Reward_Name {  get; set; }
    public float Probability { get; set; }
}
public class GachaTable : DataTable
{
    private readonly Dictionary<int, GachaData> table = new Dictionary<int, GachaData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<GachaData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Gacha_ID))
            {
                table.Add(item.Gacha_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }
    }
    public GachaData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
    public List<GachaData> Table()
    {
        return table.Values.ToList();
    }
}
