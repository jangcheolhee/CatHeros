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

    private float[] cumulativeRarityRates;
    private float totalRarityRate;
    private HashSet<int> pickedItemIds = new HashSet<int>(); // 중복 방지용

    private List<GachaData> gachaPool;
    private int currency ;
    private List<GachaData> pendingResults;




    public override void Open()
    {

        backButton.onClick.AddListener(OnBack);
        probabilityButton.onClick.AddListener(OnShowProbability);
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



                Debug.Log($"획득: {picked.Gacha_ID} (확률 {picked.Probability}%)");
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
        int cost = count == 1 ? 1 : 10; // 예시: 1회 300, 10회 2700
        if (currency < cost)
        {
            Debug.LogWarning("재화 부족!");
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
            Debug.Log($"획득 결과: {item.Gacha_ID} ({item.Probability})");
        }
        GameManager.Instance.Save();
        loadingPanel.SetActive(true);
        AudioManager.Instance.PlayRollBgm();
        resultPanel.SetActive(false);



        // 3) 몇 초 뒤에 클릭 가능하게 전환
        StartCoroutine(ShowResultsAfterDelay(2f, count)); // 2초 로딩
    }

    private IEnumerator ShowResultsAfterDelay(float delay, int count)
    {
        yield return new WaitForSeconds(delay);

        // 로딩 패널을 "클릭 대기 상태"로 전환

        // 클릭하면 결과창으로 이동
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

        // 실제 결과 UI에 출력
        //foreach (Transform child in resultPanel.transform)
        //    Destroy(child.gameObject);
        resultPanel.GetComponent<GachaResult>().ShowResult(pendingResults[0]);
        foreach (var item in pendingResults)
        {
            Debug.Log($"획득 결과: {item.Gacha_ID}");

            // TODO: 슬롯 Prefab을 Instantiate해서 아이콘/텍스트 표시
            // 예: Instantiate(resultSlotPrefab, resultPanel.transform).SetData(item);
        }


    }
    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
            currencyText.text = currency.ToString();
    }

}
