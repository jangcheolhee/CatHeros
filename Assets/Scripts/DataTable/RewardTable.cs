using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


public class RewardData
{
    public int reward_id { get; set; }
    public int RewardTarget_id { get; set; }
    public string R_item_Name { get; set; }
    public int R_item_id {  get; set; }
    public int quantity { get; set; }

}
public class RewardTable : DataTable
{
    private readonly Dictionary<int, List<RewardData>> table = new Dictionary<int, List<RewardData>>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<RewardData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.RewardTarget_id))
            {
                table.Add(item.RewardTarget_id, new List<RewardData>());
                table[item.RewardTarget_id].Add(item);
            }
            else
            {
                table[item.RewardTarget_id].Add(item);
            }

        }

    }
    public List<RewardData> Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
