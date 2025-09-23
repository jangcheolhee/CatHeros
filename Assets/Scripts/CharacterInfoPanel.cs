
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    public Image Icon;
     public TextMeshProUGUI nameText;
     public TextMeshProUGUI descText;

    public void SetData(int charId, string itemName, string itemDesc)
    {
        Icon.sprite = Resources.Load<Sprite>($"icon/{charId}");
        nameText.text = itemName;
        descText.text = itemDesc;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
