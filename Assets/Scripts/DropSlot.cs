
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    public FormationRow row;
    public int index;
    private GameObject currentIcon;
    private CharacterSelectScene characterSelectScene;
    private void Awake()
    {
        characterSelectScene = GetComponentInParent<CharacterSelectScene>();
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (characterSelectScene.isDrop)
        {
            characterSelectScene.isDrop = false;

            if (GameManager.Instance.PartySlots.Count == 5 && currentIcon == null) return;

            var icon = characterSelectScene.characterSlot;
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


                currentIcon = Instantiate(icon.gameObject, transform);
                currentIcon.transform.localPosition = Vector3.zero;


                var di = currentIcon.GetComponent<CharacterSlot>();
                if (di != null) Destroy(di);


                var slotIcon = currentIcon.AddComponent<SlotIcon>();
                slotIcon.Source = icon;
                icon.SetAssigned(true);
                di.canvasGroup.blocksRaycasts = false;
                var slotInfo = new SlotInfo
                {
                    row = row,
                    index = index,
                    characterId = icon.characterID
                };

                GameManager.Instance.PartySlots.RemoveAll(s => s.row == row && s.index == index);
                GameManager.Instance.PartySlots.Add(slotInfo);
                Debug.Log(GameManager.Instance.PartySlots.Count);

            }
        }
        else if (currentIcon != null)
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




    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.PartySlots.Count == 5 && currentIcon == null) return;

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

            di.canvasGroup.blocksRaycasts = false;
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
    public void Clear()
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
        GameManager.Instance.PartySlots.RemoveAll(s => s.row == row && s.index == index);

    }


}
