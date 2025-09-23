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

    public void SetItem(CharacterData data)
    {
        this.characterData = data;

        icon.sprite = Resources.Load<Sprite>($"icon/{data.Character_ID}" );
        textMeshProUGUI.text = data.Name;
    }
}
