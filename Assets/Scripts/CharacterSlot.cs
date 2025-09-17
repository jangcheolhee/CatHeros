using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    
    private GameObject draggingIcon;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public int characterID;

    public bool IsAssigned { get; private set; } = false; 
    private Image image;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }

    public void SetAssigned(bool assigned)
    {
        IsAssigned = assigned;
        if (image != null)
            image.color = assigned ? new Color(1, 1, 1, 0.5f) : Color.white; 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsAssigned) return; 

        
        draggingIcon = Instantiate(gameObject, canvas.transform);
       
        var cg = draggingIcon.GetComponent<CanvasGroup>();
        if (cg == null) cg = draggingIcon.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
            draggingIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
            Destroy(draggingIcon);
    }
}
