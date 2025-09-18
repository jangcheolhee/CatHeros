using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI.Table;

public class DropSlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public FormationRow row;
    public int index;
    private GameObject currentIcon;
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (currentIcon != null)
        {
            var oldIcon = currentIcon.GetComponent<SlotIcon>();
            if (oldIcon != null)
            {
                oldIcon.Source.SetAssigned(false);
            }
            Destroy(currentIcon);
            currentIcon = null;

            // GameManager에서도 제거
            GameManager.Instance.PartySlots.RemoveAll(s => s.row == row && s.index == index);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }


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
            if(GameManager.Instance.PartySlots.Count < 5)
            {
                GameManager.Instance.PartySlots.RemoveAll(s => s.row == row && s.index == index);
                GameManager.Instance.PartySlots.Add(slotInfo);
            }
            




        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
