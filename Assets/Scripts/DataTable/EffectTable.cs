using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EffectData
{
    public int Effect_ID {  get;  set; }
    public string Effect_Name { get; set; }
    public int Effect_Type { get;  set; }
    public float Tick_Interval {  get;  set; }
    public float Probability {  get;  set; }
    public int Stackable {  get;  set; }
    public string Description {  get;  set; }


}
public class EffectTable : DataTable
{
    private readonly Dictionary<int, EffectData> table = new Dictionary<int, EffectData>();

    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<EffectData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Effect_ID))
            {
                table.Add(item.Effect_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }
    }
    public EffectData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
}
