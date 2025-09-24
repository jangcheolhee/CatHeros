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

        }
        else
        {
            allData = DataTableManger.CharacterTable.Table; ;
            foreach (var character in allData)
            {
                var CharacterInfo = new CharacterInfo();
                CharacterInfo.Character_ID = DataTableManger.CharacterTable.Get(character.Character_ID);
                if(initIds.Contains(character.Character_ID))
                {
                    CharacterInfo.IsGet = true;
                }
                saveCharacterList.Add(CharacterInfo);
            }
            
        }
        SaveLoadManager.Data.CharacterInfos = saveCharacterList;
        SaveLoadManager.Save();

    }



}
