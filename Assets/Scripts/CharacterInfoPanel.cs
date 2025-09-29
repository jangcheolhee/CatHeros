
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    [Header("Header")]
    public TextMeshProUGUI titleText;
    public Button backButton;

    [Header("Character Section")]
    public Image portraitImage;
    public GameObject tankerButton;
    public GameObject normalButton;
    public GameObject specialButton;

    [Header("Description")]
    public TextMeshProUGUI descriptionText;

    [Header("Tabs")]
    public Button statTab;
    public Button skillTab;
    public Button pairBonusTab;

    [Header("Tab Contents")]
    public GameObject statPanel;
    public GameObject skillPanel;
    public GameObject pairBonusPanel;

    [Header("Stat Content UI")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI spdText;



    [Header("Skill Content UI")]
    public TextMeshProUGUI skillListText;

    [Header("Pair Bonus UI")]
    public TextMeshProUGUI pairBonusText;

    [Header("Footer")]
    public Button levelUpButton;
    public TextMeshProUGUI levelText;

    private CharacterInfo currentCharacter;

    void Start()
    {
        // 버튼 이벤트 연결
        statTab.onClick.AddListener(() => ShowTab("stat"));
        skillTab.onClick.AddListener(() => ShowTab("skill"));
        pairBonusTab.onClick.AddListener(() => ShowTab("pair"));

        backButton.onClick.AddListener(() => ClosePanel());
        levelUpButton.onClick.AddListener(() => LevelUp());
    }



    // 캐릭터 데이터 표시
    public void SetCharacter(CharacterInfo data)
    {
        gameObject.SetActive(true);
        currentCharacter = data;
        titleText.text = currentCharacter.Character_ID.Name;
        tankerButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>($"icon/{currentCharacter.Character_ID.Position}");
        tankerButton.GetComponentInChildren<TextMeshProUGUI>().text = currentCharacter.Character_ID.Position.ToString();

        normalButton.GetComponentInChildren<TextMeshProUGUI>().text = currentCharacter.Character_ID.Position.ToString();

        Debug.Log(currentCharacter.Character_ID.Character_ID);
        portraitImage.sprite = Resources.Load<Sprite>($"icon/{currentCharacter.Character_ID.Character_ID}");
        descriptionText.text = currentCharacter.Character_ID.Description;

        hpText.text = $"HP: {currentCharacter.Hp}";
        atkText.text = $"ATK: {currentCharacter.Atk}";
        defText.text = $"DEF: {currentCharacter.Def}";
        spdText.text = $"SPD: {currentCharacter.Spd}";

        skillListText.text = DataTableManger.SkillTable.Get(currentCharacter.Character_ID.Skill_Set_ID).Skill_Name;
        pairBonusText.text = DataTableManger.SkillTable.Get(currentCharacter.Character_ID.Skill_Set_ID).Skill_Name;
        levelText.text = $"exp : {currentCharacter.Exp} / {GameManager.Instance.Exp} gold {currentCharacter.Gold} / {GameManager.Instance.Gold}";
        ShowTab("stat"); // 기본 탭
    }

    private void ShowTab(string tab)
    {
        statPanel.SetActive(tab == "stat");
        skillPanel.SetActive(tab == "skill");
        pairBonusPanel.SetActive(tab == "pair");
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void LevelUp()
    {
        if (GameManager.Instance.Exp >= currentCharacter.Exp && GameManager.Instance.Gold >= currentCharacter.Gold)
        {
            GameManager.Instance.Exp -= currentCharacter.Exp;
            GameManager.Instance.Gold -= currentCharacter.Gold;
            var data = DataTableManger.LevelUpTable.Get(currentCharacter.Character_ID.Growth_Curve_ID);
            currentCharacter.Level += 1;
            currentCharacter.Hp = currentCharacter.Character_ID.Base_HP + data.HP * (currentCharacter.Level - 1);
            currentCharacter.Atk = currentCharacter.Character_ID.Base_ATK + data.ATK * (currentCharacter.Level - 1);

            currentCharacter.Def = currentCharacter.Character_ID.Base_DEF + data.DEF * (currentCharacter.Level - 1);
            currentCharacter.Spd = currentCharacter.Character_ID.Base_SPD + (int)(data.SPD * (currentCharacter.Level - 1));
            switch (currentCharacter.Character_ID.Rarity)
            {
                case 1:
                    currentCharacter.Exp = (int)(100 * Math.Pow(currentCharacter.Level, 1.2) * 1.0);
                    currentCharacter.Gold = (int)(100 * Math.Pow(currentCharacter.Level, 1.3) * 1.0);

                    break;
                case 2:
                    currentCharacter.Exp = (int)(100 * Math.Pow(currentCharacter.Level, 1.2) * 1.2);
                    currentCharacter.Gold = (int)(100 * Math.Pow(currentCharacter.Level, 1.3) * 1.2);

                    break;
                case 3:
                    currentCharacter.Exp = (int)(100 * Math.Pow(currentCharacter.Level, 1.2) * 1.5);
                    currentCharacter.Gold = (int)(100 * Math.Pow(currentCharacter.Level, 1.3) * 1.5);

                    break;
                case 4:
                    currentCharacter.Exp = (int)(100 * Math.Pow(currentCharacter.Level, 1.2) * 2.0);
                    currentCharacter.Gold = (int)(100 * Math.Pow(currentCharacter.Level, 1.3) * 2.0);

                    break;
            }
            hpText.text = $"HP: {currentCharacter.Hp}";
            atkText.text = $"ATK: {currentCharacter.Atk}";
            defText.text = $"DEF: {currentCharacter.Def}";
            spdText.text = $"SPD: {currentCharacter.Spd}";


            GameManager.Instance.Save();
            levelText.text = $"exp : {currentCharacter.Exp} / {GameManager.Instance.Exp} gold {currentCharacter.Gold} / {GameManager.Instance.Gold}";

            var slotList = FindObjectOfType<UiCollectSlotList>();
            if (slotList != null) slotList.Load();
        }

    }
}
