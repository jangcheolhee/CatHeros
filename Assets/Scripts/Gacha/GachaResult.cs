using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResult : MonoBehaviour
{

    public GameObject resultPanel;
    public Image pos;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public Button confirmButton;
    public Image icon;

    private void Start()
    {
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void ShowResult(GachaData data)
    {
        if(data.Gacha_Type == 1)
        {
            resultPanel.SetActive(true);
            pos.sprite = Resources.Load<Sprite>($"icon/{DataTableManger.CharacterTable.Get(data.Result_ID).Position}");
            nameText.text = data.Reward_Name;
     
            switch (data.Rarity)
            {
                case 1: rarityText.text = "N"; break;
                case 2: rarityText.text = "R"; break;
                case 3: rarityText.text = "SR"; break;
                case 4: rarityText.text = "SSR"; break;
            }
        }
        else
        {
            resultPanel.SetActive(false);
        }
            icon.sprite = Resources.Load<Sprite>($"icon/{data.Result_ID}");
            gameObject.SetActive(true);
    }
}
