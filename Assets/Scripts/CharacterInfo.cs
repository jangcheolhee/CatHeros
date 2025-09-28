using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class
    CharacterInfo
{
    public Guid instanceId;
    public DateTime creationTime;

    [JsonConverter(typeof(CharacterDataConvertor))]
    public CharacterData Character_ID;
    
    
    public int Level;
    public int Exp;
    public bool IsGet;
    public int Hp;
    public int Atk;
    public int Def;
    public int Spd;
    public CharacterInfo()
    {
        instanceId = Guid.NewGuid();
        creationTime = DateTime.Now;
    }
}

[Serializable]
public class PartySlot
{
    public FormationRow Row; // Front / Rear
    public int Index;        // 위치 인덱스
    public int CharacterId;  // 배치된 캐릭터
}

