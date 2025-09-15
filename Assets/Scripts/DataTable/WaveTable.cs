using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public int Monster_ID { get; set; }
    public int Count { get; set; }
    public string Position { get; set; }
}
public class WaveData
{
    public int Wave_ID {  get;  set; }
    public int Stage_ID { get;  set; }
    public string Enemy_ID {  get;  set; }
    public int Wave_Number {  get; set; }
    public List<EnemyData> Enemies
    {
        get
        {
            if (string.IsNullOrEmpty(Enemy_ID))
                return new List<EnemyData>();

            return JsonConvert.DeserializeObject<List<EnemyData>>(Enemy_ID);
        }
    }
    public int Spawn_Delay {  get; set; }

}
public class WaveTable : DataTable
{
    private readonly Dictionary<int, List<WaveData>> table = new Dictionary<int, List<WaveData>>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<WaveData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Stage_ID))
            {
                table.Add(item.Stage_ID, new List<WaveData>());
                table[item.Stage_ID].Add(item);
            }
            else
            {
                table[item.Stage_ID].Add(item);
            }

        }

    }
    public List<WaveData> Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
