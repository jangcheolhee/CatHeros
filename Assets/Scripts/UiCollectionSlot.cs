using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCollectionSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI textMeshProUGUI;
    public int slotIdx;
    public CharacterData characterData;

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
    }
}
