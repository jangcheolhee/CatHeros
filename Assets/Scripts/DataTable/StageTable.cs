using UnityEngine;
using System.Collections.Generic;

public class StageData
{
    public int StageID {  get; set; }
    public string StageName { get; set; } 
    public string BossID {  get; set; }
    public int RewardExp {  get; set; }
    public int MaxWaveCount{get; set; }
    public string SpecialCondition {  get; set; }
    public string Story_Link_ID {  get; set; }
    public string Description {  get; set; }

}
public class StageTable : DataTable
{
    private readonly Dictionary<int, StageData> table = new Dictionary<int, StageData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<StageData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.StageID))
            {
                table.Add(item.StageID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }

    }
    public StageData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
