using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiCollectSlotList : MonoBehaviour
{
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
        //var list = itemList.Where(filterings[(int)Filtering]).ToList();
        //list.Sort(comparison[(int)sorting]);


        for (int i = slotList.Count; i < saveCharacterList.Count; ++i)
        {
            UiCollectionSlot newSlot;
            if (saveCharacterList[i].IsGet)
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
                //onSelectSlot?.Invoke(newSlot.itemData);
            });
            slotList[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < slotList.Count; i++)
        {
            if (i < saveCharacterList.Count)
            {
                slotList[i].SetItem(saveCharacterList[i].Character_ID);
                slotList[i].gameObject.SetActive(true);
            }
            else
            {
                slotList[i].SetEmpty();
                slotList[i].gameObject.SetActive(false);
            }
        }

        selectedSlotIndex = -1;
        //onUpdateSlots?.Invoke();
    }
}
