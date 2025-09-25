
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
    public Button tankerButton;
    public Button normalButton;
    public Button specialButton;

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

    [Header("Skill Content UI")]
    public TextMeshProUGUI skillListText;

    [Header("Pair Bonus UI")]
    public TextMeshProUGUI pairBonusText;

    [Header("Footer")]
    public Button levelUpButton;

    private CharacterData currentCharacter;

    void Start()
    {
        // 버튼 이벤트 연결
        statTab.onClick.AddListener(() => ShowTab("stat"));
        skillTab.onClick.AddListener(() => ShowTab("skill"));
        pairBonusTab.onClick.AddListener(() => ShowTab("pair"));

        backButton.onClick.AddListener(() => ClosePanel());
        //levelUpButton.onClick.AddListener(() => LevelUp());
    }

    // 캐릭터 데이터 표시
    public void SetCharacter(CharacterInfo data)
    {
        gameObject.SetActive(true);
        currentCharacter = data.Character_ID;
        titleText.text = currentCharacter.Name;
        portraitImage.sprite = Resources.Load<Sprite>($"icon/{currentCharacter.Character_ID}");
        descriptionText.text = currentCharacter.Description;

        //hpText.text = $"HP: {data.hp}";
        //atkText.text = $"ATK: {data.atk}";

        //skillListText.text = string.Join("\n", data.skills);
        //pairBonusText.text = data.pairBonus;

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

    //private void LevelUp()
    //{
    //    if (currentCharacter != null)
    //    {
    //        currentCharacter.hp += 10;
    //        currentCharacter.atk += 2;
    //        hpText.text = $"HP: {currentCharacter.hp}";
    //        atkText.text = $"ATK: {currentCharacter.atk}";
    //        Debug.Log($"{currentCharacter.name} 레벨 업!");
    //    }
    //}
}
