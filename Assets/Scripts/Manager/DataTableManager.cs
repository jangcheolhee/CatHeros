using System.Collections.Generic;
using UnityEngine;

public static class DataTableManger
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    static DataTableManger()
    {
        Init();
        
    }

    private static void Init()
    {
        var charcterTable = new CharacterTable();
        charcterTable.Load(DataTableIds.Character);
        tables.Add(DataTableIds.Character, charcterTable);
        var skillTable = new SkillDataTable();
        skillTable.Load(DataTableIds.Skill);
        tables.Add(DataTableIds.Skill, skillTable);
        var stageTable = new StageTable();
        stageTable.Load(DataTableIds.Stage);
        tables.Add(DataTableIds.Stage, stageTable);
        var waveTable = new WaveTable();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);

        var monsterTable = new MonsterTable();
        monsterTable.Load(DataTableIds.Monster);
        tables.Add(DataTableIds.Monster, monsterTable);

        var effectTable = new EffectTable();
        effectTable.Load(DataTableIds.Effect);
        tables.Add (DataTableIds.Effect, effectTable);
        var rewardTable = new RewardTable();
        rewardTable.Load(DataTableIds.Reward);
        tables.Add(DataTableIds.Reward, rewardTable);

        var itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);
#if UNITY_EDITOR


#else
        //var stringTable = new StringTable();
        //stringTable.Load(DataTableIds.String);
        //tables.Add(DataTableIds.String, stringTable);
#endif
    }

    public static CharacterTable CharacterTable
    {
        get
        {
            return Get<CharacterTable>(DataTableIds.Character);
        }
    }
    public static SkillDataTable SkillTable
    {
        get
        {
            return Get<SkillDataTable>(DataTableIds.Skill);
        }
    }
    public static StageTable StageTable
    {
        get
        {
            return Get<StageTable>(DataTableIds.Stage);
        }
    }
    public static WaveTable WaveTable
    {
        get
        {
            return Get<WaveTable>(DataTableIds.Wave);
        }
    }
    public static MonsterTable MonsterTable
    {
        get
        {
            return Get<MonsterTable>(DataTableIds.Monster);
        }
    }
    public static EffectTable EffectTable
    {
        get
        {
            return Get<EffectTable>(DataTableIds.Effect);
        }
    }
    public static RewardTable RewardTable
    {
        get
        {
            return Get<RewardTable>(DataTableIds.Reward);
        }
    }

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(DataTableIds.Item);
        }
    }
    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("���̺� ����");
            return null;
        }
        return tables[id] as T;
    }
}
