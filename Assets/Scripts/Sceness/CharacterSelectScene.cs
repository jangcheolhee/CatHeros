using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectScene : MonoBehaviour
{


    private List<int> charIds ;
    public GameObject iconPrefab;
    public Transform scrollViewContent;
    public Sprite spriteIcon;
    public EnemyInfoPanel enemyInfoPanel;
    public CharacterInfoPanel characterInfoPanel;
    public bool isDrop = false;
    public CharacterSlot characterSlot;

    private void Start()
    {
        foreach(var cha in GameManager.Instance.saveCharacterList)
        {
            if(cha.IsGet)
            {
                charIds.Add(cha.Character_ID.Character_ID);
            }
        }
        GameManager.Instance.PartySlots.Clear();
        foreach (int charId in charIds)
        {
            GameObject icon = Instantiate(iconPrefab, scrollViewContent);
            CharacterSlot charSlot = icon.GetComponent<CharacterSlot>();
            charSlot.characterSelectScene = this;
            charSlot.characterID = charId;
            spriteIcon = Resources.Load<Sprite>($"icon/{charId}");
           
            charSlot.icon.sprite = spriteIcon;
            icon.GetComponentInChildren<TextMeshProUGUI>().text = DataTableManger.CharacterTable.Get(charId).Name;
        }
    }
    public void OnClickConfirm()
    {
        if (GameManager.Instance.PartySlots.Count == 0)
        {
            return;
        }

        SceneManager.LoadScene("Game");
    }
    public void OnClickBack()
    {
        SceneManager.LoadScene("StageSelect");
    }
    public void OnClickEnemy()
    {
        enemyInfoPanel.Show();
    }
    public void OnClickCharacter(int id, string name, string desc)
    {
        characterInfoPanel.SetData(id, name,desc);
    }
    public void OnClickClear()
    {
        var slots = GetComponentsInChildren<DropSlot>();
        foreach (DropSlot slot in slots)
        {
            slot.Clear();
        }
    }
    public void Select(CharacterSlot slot)
    {
        if(!isDrop )
        {
            isDrop = true;
        }
    }

}
