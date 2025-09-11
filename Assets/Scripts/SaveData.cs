using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public List<CharacterInfo> CharacterInfos { get; protected set; } = new List<CharacterInfo>();
    public SaveDataV1()
    {
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        throw new NotImplementedException();
    }
}
