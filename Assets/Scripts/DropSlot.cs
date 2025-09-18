using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI.Table;

public class DropSlot : MonoBehaviour, IDropHandler
{

    public FormationRow row;
    public int index;
    private GameObject currentIcon; // 슬롯에 있는 아이콘

    public void OnDrop(PointerEventData eventData)
    {
        var icon = eventData.pointerDrag.GetComponent<CharacterSlot>();
        if (icon != null && !icon.IsAssigned)
        {
            
            if (currentIcon != null)
            {
                var oldIcon = currentIcon.GetComponent<SlotIcon>();
                if (oldIcon != null)
                {
                    oldIcon.Source.SetAssigned(false); 
                }
                Destroy(currentIcon);
            }

    
            currentIcon = Instantiate(eventData.pointerDrag.gameObject, transform);
            currentIcon.transform.localPosition = Vector3.zero;

            
            var di = currentIcon.GetComponent<CharacterSlot>();
            if (di != null) Destroy(di);

         
            var slotIcon = currentIcon.AddComponent<SlotIcon>();
            slotIcon.Source = icon;
            icon.SetAssigned(true);

            var slotInfo = new SlotInfo
            {
                row = row,
                index = index,
                characterId = icon.characterID
            };

            GameManager.Instance.PartySlots.RemoveAll(s => s.row == row && s.index == index);
            GameManager.Instance.PartySlots.Add(slotInfo);




        }
    }
}
