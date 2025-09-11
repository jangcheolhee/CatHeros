using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class CharacterInfo 
{
    public Guid instanceId;
    public DateTime creationTime;

    [JsonConverter(typeof(CharacterDataConvertor))]
    public CharacterData Character_ID;
    public int Level {  get; set; }
    public CharacterInfo()
    {
        instanceId = new Guid();
        creationTime = DateTime.Now;
    }
}
