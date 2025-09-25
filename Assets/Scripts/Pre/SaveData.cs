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
    

    public List<CharacterInfo> CharacterInfos { get; set; } = new List<CharacterInfo>();
    //public List<PartySlot> PartySlots { get; set; } = new List<PartySlot>();

    // ¿Á»≠
    public int ClearStage { get; set; }
    public int Gold { get; set; }
    public int Exp { get; set; }
    public int Chur { get; set; }
    public int Yarn {  get; set; }

    public SaveDataV1()
    {
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}
