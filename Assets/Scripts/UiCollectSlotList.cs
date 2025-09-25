using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiCollectSlotList : MonoBehaviour
{
    public enum SortingOption
    {

        NameAcceding,
        NameDeccending,
        RarityAccending,
        RarityDeccending,
        LevelAccending,
        LevelDeccending,
        PossitionAccending,
        PossitionDeccending,

    }
    // (lhs, rhs) => { return lhs.creationTime.CompareTo(rhs.creationTime);},
    //(lhs, rhs) => { return rhs.creationTime.CompareTo(lhs.creationTime);},
    public readonly System.Comparison<CharacterInfo>[] comparison = {

        (lhs, rhs) => { return lhs.Character_ID.Name.CompareTo(rhs.Character_ID.Name);},
        (lhs, rhs) => { return rhs.Character_ID.Name.CompareTo(lhs.Character_ID.Name);},
        (lhs, rhs) => { return lhs.Character_ID.Rarity.CompareTo(rhs.Character_ID.Rarity);},
        (lhs, rhs) => { return rhs.Character_ID.Rarity.CompareTo(lhs.Character_ID.Rarity);},
        (lhs, rhs) => { return lhs.Level.CompareTo(rhs.Level);},
        (lhs, rhs) => { return rhs.Level.CompareTo(lhs.Level);},
        (lhs, rhs) => { return lhs.Character_ID.Position.CompareTo(rhs.Character_ID.Position);},
        (lhs, rhs) => { return rhs.Character_ID.Position.CompareTo(lhs.Character_ID.Position);},

    };

    private SortingOption sorting = SortingOption.NameAcceding;
    public SortingOption Sorting
    {
        get => sorting;
        set
        {
            sorting = value;
            UpdateSlots(saveCharacterList);
        }
    }
    //public UnityEvent onUpdateSlots;
    //public UnityEvent<CharacterInfo> onSelectSlot;
    public int SlotIndex { get; set; }

    public UiCollectionSlot prefab;
    public ScrollRect getRect;
    public ScrollRect lockRect;
    private List<UiCollectionSlot> slotList = new List<UiCollectionSlot>();
    private List<CharacterData> allData;

    private List<CharacterInfo> saveCharacterList = new List<CharacterInfo>();

    public int maxCount = 30;
    private int itemCount = 0;
    private int selectedSlotIndex = -1;
    public void Save()
    {
       

        SaveLoadManager.Data.CharacterInfos = saveCharacterList;
        SaveLoadManager.Save();

    }

    public void Load()
    {
        if (SaveLoadManager.Load())
        {
            allData = DataTableManger.CharacterTable.Table; ;
            saveCharacterList = SaveLoadManager.Data.CharacterInfos;

        }
        UpdateSlots(saveCharacterList);
    }


    private void OnEnable()
    {
        Load();
    }
    private void OnDisable()
    {
        Save();
    }
    private void UpdateSlots(List<CharacterInfo> saveCharacterList)
    {


        var list = saveCharacterList.Where(x => true).ToList();

        list.Sort(comparison[(int)sorting]);
        int count = 0;
        foreach (var item in saveCharacterList)
        {
            if (item.IsGet) count++;
        }
        for (int i = 0; i < list.Count; ++i)
        {
            UiCollectionSlot newSlot;
            if (i < count)
            { 
                newSlot = Instantiate(prefab, getRect.content); 
            }
            else
            {
                newSlot = Instantiate(prefab, lockRect.content);
            }
            newSlot.SetEmpty();
            newSlot.slotIdx = i;
            slotList.Add(newSlot);

            var button = newSlot.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                selectedSlotIndex = newSlot.slotIdx;
            });
            slotList[i].gameObject.SetActive(false);
        }

        int up = 0;
        int down = count;
        for (int i = 0; i < slotList.Count; i++)
        {
            if (i < list.Count)
            {
                if (list[i].IsGet)
                {
                    slotList[up].SetItem(list[i].Character_ID);
                    slotList[up].gameObject.SetActive(true);
                    up++;
                }
                else
                {
                    slotList[down].SetItem(list[i].Character_ID);
                    slotList[down].gameObject.SetActive(true);
                    down++;
                }


            }
            else
            {
                slotList[i].SetEmpty();
                slotList[i].gameObject.SetActive(false);
            }
        }
        saveCharacterList = list;
        selectedSlotIndex = -1;
        //onUpdateSlots?.Invoke();
    }
}
