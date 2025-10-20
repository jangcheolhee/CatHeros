using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaScene : GenericWindow
{

    [Header("UI 연결")]
    public Button backButton;
    public TextMeshProUGUI currencyText;
    public GameObject gachaPanel;
    public Button probabilityButton;
    public GameObject probabilityPanel;
    public Button rollOneButton;
    public Button rollTenButton;
    public GameObject loadingPanel;
    public GameObject resultPanel;
    public TenResultPanel resultTenPanel;

    
    

    private List<GachaData> gachaPool;
    private int currency ;
    private List<GachaData> pendingResults;




    public override void Open()
    {

        backButton.onClick.AddListener(OnBack);
        probabilityButton.onClick.AddListener(OnShowProbability);
        rollOneButton.onClick.RemoveAllListeners();
        rollTenButton.onClick.RemoveAllListeners();
        rollOneButton.onClick.AddListener(() => OnRoll(1));
        rollTenButton.onClick.AddListener(() => OnRoll(10));
        gachaPool = DataTableManger.GachaTable.Table();
        AudioManager.Instance.PlayGachaBgm();
        currency = GameManager.Instance.Chur;
        UpdateCurrencyUI();
        base.Open();

    }
    private void OnBack()
    {
        gachaPanel.SetActive(false);
        Debug.Log("뒤로가기");
    }
    private void OnShowProbability()
    {
        probabilityPanel.SetActive(true);
        Debug.Log("뽑기 확률 팝업 열림");
    }

    private GachaData Roll()
    {
        if (gachaPool.Count == 0)
            return null;

        // 누적 확률표 생성
        float total = 0f;
        List<float> cumulative = new List<float>();
        foreach (var item in gachaPool)
        {
            total += item.Probability;
            cumulative.Add(total);
        }

        // 랜덤 추첨
        float roll = UnityEngine.Random.Range(0f, total);
        for (int i = 0; i < cumulative.Count; i++)
        {
            if (roll <= cumulative[i])
            {
                var picked = gachaPool[i];
                return picked;
            }
        }

        return null;
    }
    private List<GachaData> RollMulti(int count)
    {
        List<GachaData> results = new List<GachaData>();

        for (int i = 0; i < count; i++)
        {
            var item = Roll();
            if (item != null)
                results.Add(item);
            else
                break; // 뽑을 아이템 없음
        }

        return results;
    }
    private void OnRoll(int count)
    {
        int cost = count == 1 ? 1 : 10; 
        if (currency < cost)
        {
           
            return;
        }

        currency -= cost;
        GameManager.Instance.Chur = currency;
        UpdateCurrencyUI();

        pendingResults = RollMulti(count);

        foreach (var item in pendingResults)
        {
            switch (item.Gacha_Type)
            {
                case 1:
                    var target = GameManager.Instance.saveCharacterList
                     .FirstOrDefault(c => c.Character_ID.Character_ID == item.Result_ID);

                    if (target != null)
                    {
                        if (!target.IsGet)
                        {
                            GameManager.Instance.UpdateCharacter(target);
                        }
                        else
                        {
                            // 이미 소유 → 보너스 처리 (예: 골드 변환)
                            GameManager.Instance.Gold += 1000;
                            Debug.Log($"중복 캐릭터 → 골드 1000 지급");
                        }
                    }
                    break;
                case 2:
                    GameManager.Instance.Gold += item.Amount;
                    break;
                case 3:
                    GameManager.Instance.Exp += item.Amount;
                    break;
                case 4:
                    GameManager.Instance.Yarn += item.Amount;
                    break;
            }
            Debug.Log($"획득 결과: {item.Gacha_ID} ({item.Probability}) {item.Amount}");
        }
        GameManager.Instance.Save();
        loadingPanel.SetActive(true);
        AudioManager.Instance.PlayRollBgm();
        resultPanel.SetActive(false);



        ShowResultsAfterDelay( count); 
    }

    private void ShowResultsAfterDelay(int count)
    {
        
        var btn = loadingPanel.GetComponentInChildren<UnityEngine.UI.Button>();
        btn.onClick.RemoveAllListeners();
        if (count == 1)
            btn.onClick.AddListener(ShowResultPanel);
        else
        {
            btn.onClick.AddListener(() => loadingPanel.SetActive(false));
            btn.onClick.AddListener(() => resultTenPanel.OpenAndShow(pendingResults));
        }

    }

    private void ShowResultPanel()
    {
        loadingPanel.SetActive(false);
        resultPanel.SetActive(true);

     
        resultPanel.GetComponent<GachaResult>().ShowResult(pendingResults[0]);



    }
    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
            currencyText.text = currency.ToString();
    }
    public void OnClickX()
    {
        probabilityPanel.SetActive(false);
    }
}
