using UnityEngine;
using System.Collections.Generic;

public class ItemData
{
    public int Item_ID {  get; set; }
    public string Item_Name { get; set; }
    public int Max_Stack {  get; set; }
    public string Description {  get; set; }


}
public class ItemTable : DataTable
{
    private readonly Dictionary<int, ItemData> table = new Dictionary<int, ItemData>();
    public override void Load(string filename)
    {
        table.Clear();
        var path = string.Format(FormatPath, filename);
        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<ItemData>(textAsset.text);
        foreach (var item in list)
        {
            if (!table.ContainsKey(item.Item_ID))
            {
                table.Add(item.Item_ID, item);
            }
            else
            {
                Debug.LogError("캐릭터 ID 아이디 중복!");
            }

        }

    }
    public ItemData Get(int id)
    {

        if (!table.ContainsKey(id))
        {
            return null;
        }
        return table[id];
    }
   
}
