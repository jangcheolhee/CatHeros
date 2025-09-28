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
    public int Gold {  get; set; }
    public int Chur {  get; set; }
    public int Exp {  get; set; }
    public int Yarn {  get; set; }

    public List<CharacterInfo> saveCharacterList = new List<CharacterInfo>();
    private List<CharacterData> allData;
    private int[] initIds = new int[] { 10101,10102,10103,10104,10105};
    
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
            
        }
        SaveLoadManager.Data.CharacterInfos = saveCharacterList;
        SaveLoadManager.Save();

    }
    public void Save()
    {
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
    }
    

}
