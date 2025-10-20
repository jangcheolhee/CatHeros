using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FormationRow { Front, Rear }

[System.Serializable]
public class SlotInfo
{
    public FormationRow row;
    public int index;
    public int characterId;
}

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    public int SelectedStageId = -1;
    public List<SlotInfo> PartySlots = new List<SlotInfo>();
    public int ClearStage {  get; set; }
    public event Action OnCurrencyChanged;

    public string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnCurrencyChanged?.Invoke();
        }
    }
    public int Exp
    {
        get => exp;
        set
        {
            exp = value;
            OnCurrencyChanged?.Invoke();
        }
    }
    private int exp;

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnCurrencyChanged?.Invoke();
        }
    }
    private int gold;
    private int chur;
    public int Chur {
        get => chur;
        set
        {
            chur = value;
            OnCurrencyChanged?.Invoke();
        }
    }
    
    public int Yarn {  get; set; }

    public List<CharacterInfo> saveCharacterList = new List<CharacterInfo>();
    private List<CharacterData> allData;
    private int[] initIds = new int[] { 10101,10102,10103,10104,10105};

    public Windows WindowToOpenOnReturn = Windows.Main;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Load();
    }
   
    public void Load()
    {
        if (SaveLoadManager.Load())
        {
            
            saveCharacterList = SaveLoadManager.Data.CharacterInfos;
            ClearStage = SaveLoadManager.Data.ClearStage;
            Name = SaveLoadManager.Data.PlayerName;
            Gold = SaveLoadManager.Data.Gold;
            Chur = SaveLoadManager.Data.Chur;
            Exp = SaveLoadManager.Data.Exp;
            Yarn = SaveLoadManager.Data.Yarn;

        }
        else
        {
            allData = DataTableManger.CharacterTable.Table; 
            foreach (var character in allData)
            {
                var CharacterInfo = new CharacterInfo();
                CharacterInfo.Character_ID = DataTableManger.CharacterTable.Get(character.Character_ID);
                if(initIds.Contains(character.Character_ID))
                {
                    UpdateCharacter(CharacterInfo);
                    
                }
                saveCharacterList.Add(CharacterInfo);
            }
            SaveLoadManager.Data.PlayerName = "User1";


        }
        SaveLoadManager.Data.CharacterInfos = saveCharacterList;
        SaveLoadManager.Save();

    }
    public void Save()
    {
        SaveLoadManager.Data.PlayerName = Name;
        SaveLoadManager.Data.ClearStage = ClearStage;
        SaveLoadManager.Data.CharacterInfos = saveCharacterList;
        SaveLoadManager.Data.Yarn = Yarn;
        SaveLoadManager.Data.Exp = Exp;
        SaveLoadManager.Data.Gold = Gold;
        SaveLoadManager.Data.Chur = Chur;
        SaveLoadManager.Save();
    }
    public void UpdateCharacter(CharacterInfo characterInfo)
    {
        characterInfo.IsGet = true;
        characterInfo.Level = 1;
        characterInfo.Hp = characterInfo.Character_ID.Base_HP;
        characterInfo.Atk = characterInfo.Character_ID.Base_ATK;
        characterInfo.Def = characterInfo.Character_ID.Base_DEF;
        characterInfo.Spd = characterInfo.Character_ID.Base_SPD; 
        switch (characterInfo.Character_ID.Rarity)
        {
            case 1:
                characterInfo.Exp = (int)(100 * Math.Pow(characterInfo.Level, 1.2) * 1.0);
                characterInfo.Gold = (int)(100 * Math.Pow(characterInfo.Level, 1.3) * 1.0);

                break;
            case 2:
                characterInfo.Exp = (int)(100 * Math.Pow(characterInfo.Level, 1.2) * 1.2);
                characterInfo.Gold = (int)(100 * Math.Pow(characterInfo.Level, 1.3) * 1.2);

                break;
            case 3:
                characterInfo.Exp = (int)(100 * Math.Pow(characterInfo.Level, 1.2) * 1.5);
                characterInfo.Gold = (int)(100 * Math.Pow(characterInfo.Level, 1.3) * 1.5);

                break;
            case 4:
                characterInfo.Exp = (int)(100 * Math.Pow(characterInfo.Level, 1.2) * 2.0);
                characterInfo.Gold = (int)(100 * Math.Pow(characterInfo.Level, 1.3) * 2.0);

                break;
        }
    }
    

}
