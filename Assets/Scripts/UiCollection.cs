using TMPro;
using UnityEngine;

public class UiCollection : MonoBehaviour
{
    public TMP_Dropdown sorting;

    public UiCollectSlotList slotList;

    private void OnEnable()
    {
        OnChangedSorting(sorting.value);
    }

    public void OnChangedSorting(int index)
    {
        if (index - 1 < 0) return;

        slotList.Sorting = (UiCollectSlotList.SortingOption)index - 1;
        
    }

  
}
