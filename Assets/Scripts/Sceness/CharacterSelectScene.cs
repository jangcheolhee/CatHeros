using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectScene : MonoBehaviour
{


    private int[] charIds = new int[] { 10101, 10102, 10103, 10104, 10105 };
    public GameObject iconPrefab;
    public Transform scrollViewContent;
    public Sprite spriteIcon;

    private void Start()
    {
        GameManager.Instance.PartySlots.Clear();
        foreach (int charId in charIds)
        {
            GameObject icon = Instantiate(iconPrefab, scrollViewContent);
            CharacterSlot charSlot = icon.GetComponent<CharacterSlot>();
            charSlot.characterID = charId;
            spriteIcon = Resources.Load<Sprite>($"icon/{charId}");
           
            charSlot.icon.sprite = spriteIcon;
            icon.GetComponentInChildren<TextMeshProUGUI>().text = DataTableManger.CharacterTable.Get(charId).Name;
        }
    }
    public void OnClickConfirm()
    {
        //if (GameManager.Instance.PartySlots.Count == 0)
        //{
        //    Debug.LogWarning("파티가 비어 있음!");
        //    return;
        //}

        SceneManager.LoadScene("Game");
    }
    public void OnClickBack()
    {
        SceneManager.LoadScene("Main");
    }

}
