using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCollectionSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI textMeshProUGUI;
    public int slotIdx;
    public CharacterData characterData;
    private CollectionScene scene;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        scene = GetComponentInParent<CollectionScene>();
     
    }

    public void SetEmpty()
    {
        icon.sprite = null;
        textMeshProUGUI.text = string.Empty;
    }

    public void SetItem(CharacterData data, bool isGet)
    {
        this.characterData = data;
        if (isGet)
        {
            icon.sprite = Resources.Load<Sprite>($"icon/{data.Character_ID}");
        }else
        {
            icon.sprite = Resources.Load<Sprite>($"icon/No");
        }
        
        textMeshProUGUI.text = data.Name;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => scene.OnClickPanel(SaveLoadManager.Data.CharacterInfos.FirstOrDefault(c => characterData.Character_ID == c.Character_ID.Character_ID)));

    }
   
}
