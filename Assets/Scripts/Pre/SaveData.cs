using System;
using System.Collections.Generic;


[Serializable]
public abstract class SaveData
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}
[Serializable]
public class SaveDataV1 : SaveData
{
    public string PlayerName { get; set; } = string.Empty;
    public int ClearStage {  get; set; }
    public List<CharacterInfo> CharacterInfos { get; set; } = new List<CharacterInfo>();
    //public List<PartySlot> PartySlots { get; set; } = new List<PartySlot>();

    // ¿Á»≠
    public int Gold { get; set; }
    public int Crystal { get; set; }
    
    public SaveDataV1()
    {
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}
