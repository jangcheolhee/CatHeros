
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    
    private CharacterSelectScene characterSelectScene;
    private GameObject draggingIcon;
    public CanvasGroup canvasGroup;
    private Canvas canvas;
    public Image icon;
    public int characterID;

    public bool IsAssigned { get; private set; } = false; 
    private Image image;
    private bool isDragging = false;
    private float pointerDownTime;


    public float longPressThreshold = 0.5f; // 길게 누르기 시간 (초)
    private float dragThreshold = 10f;
    private bool isPointerDown = false;
    private Vector2 startPos;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        characterSelectScene = GetComponentInParent<CharacterSelectScene>();
    }

    public void SetAssigned(bool assigned)
    {
        IsAssigned = assigned;
        if (image != null)
        {

            image.color = assigned ? new Color(1, 1, 1, 0.5f) : Color.white;
            
        }
    }
    void Update()
    {
        if (isPointerDown && !isDragging)
        {
            float heldTime = Time.time - pointerDownTime;
            if (heldTime >= longPressThreshold)
            {
                Debug.Log("Long Press (hold detected in Update)");
                isPointerDown = false; // 한 번만 실행되게
                OnLongPress();
            }
        }
    }
    private void OnLongPress()
    {
        characterSelectScene.OnClickCharacter(characterID, DataTableManger.CharacterTable.Get(characterID).Name, DataTableManger.CharacterTable.Get(characterID).Name);

    }
    

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging && Vector2.Distance(eventData.position, startPos) > dragThreshold)
        {
            isDragging = true;
            if (IsAssigned) return;


            draggingIcon = Instantiate(gameObject, canvas.transform);

            var cg = draggingIcon.GetComponent<CanvasGroup>();
            if (cg == null) cg = draggingIcon.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
        }

        if (isDragging)
        {
            if (draggingIcon != null)
                draggingIcon.transform.position = eventData.position;
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        isDragging = false;
        pointerDownTime = Time.time;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            if (draggingIcon != null)
                Destroy(draggingIcon);
        }
        else
        {
            float heldTime = Time.time - pointerDownTime;
            if (heldTime >= longPressThreshold)
            {
                
                Debug.Log("Long Press");
            }
            else
            {
                if(!characterSelectScene.isDrop)
                {
                    characterSelectScene.characterSlot = this;
                    characterSelectScene.isDrop = true;
                }
                Debug.Log("Tap");
            }
        }

        isPointerDown = false;
        isDragging = false;
       
    }
}
