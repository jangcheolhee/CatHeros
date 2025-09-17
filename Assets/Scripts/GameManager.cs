using System.Collections.Generic;
using UnityEngine;

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

    public List<SlotInfo> PartySlots = new List<SlotInfo>();

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
    }
}
