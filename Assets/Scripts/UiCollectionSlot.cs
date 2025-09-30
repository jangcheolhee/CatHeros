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

    public TextMeshProUGUI level;
    public Image position;
    public TextMeshProUGUI rarity;
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
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>($"icon/Lock");
        }

        textMeshProUGUI.text = data.Name;
        level.text = GameManager.Instance.saveCharacterList.FirstOrDefault(c => c.Character_ID.Character_ID == characterData.Character_ID).Level.ToString();
        switch (characterData.Rarity)
        {
            case 1:
                rarity.text = "N";
                break;
            case 2:
                rarity.text = "R";
                break;
            case 3:
                rarity.text = "SR";
                break;
            case 4:
                rarity.text = "SSR";
                break;
        }
        position.sprite = Resources.Load<Sprite>($"icon/{data.Position}");
        //button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => scene.OnClickPanel(GameManager.Instance.saveCharacterList.FirstOrDefault(c => characterData.Character_ID == c.Character_ID.Character_ID)));

    }

}
