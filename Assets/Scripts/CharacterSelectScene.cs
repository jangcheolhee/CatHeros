using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectScene : MonoBehaviour
{


    private int[] charIds = new int[] { 10101, 10102, 10103, 10104, 10105, 10101, 10102, 10103, 10104, 10105 };
    public GameObject iconPrefab;
    public Transform scrollViewContent;

    private void Start()
    {

        foreach (int charId in charIds)
        {
            GameObject icon = Instantiate(iconPrefab, scrollViewContent);
            CharacterSlot charSlot = icon.GetComponent<CharacterSlot>();
            charSlot.characterID = charId;

            icon.GetComponentInChildren<TextMeshProUGUI>().text = DataTableManger.CharacterTable.Get(charId).Name;
        }
    }

}
