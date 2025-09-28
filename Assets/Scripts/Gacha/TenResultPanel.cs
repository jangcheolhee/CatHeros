using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TenResultPanel : MonoBehaviour
{
    [Header("Refs")]
    public Transform grid;            // GridLayoutGroup가 붙은 오브젝트
    public GachaResultSlot slotPrefab;     // 위에서 만든 프리팹
    public Button confirmButton;      // 확인 버튼
    public Button skipArea;           // 연출 중 스킵하고 싶으면 임의 버튼(패널 전체)

    [Header("Reveal Settings")]
    public float revealInterval = 0.5f; // 각 슬롯 등장 간격 (요청하신 0.5초)
    public float popAnimTime = 0.15f;   // 슬롯 하나가 팝업되는 시간

    private List<GachaResultSlot> spawned = new List<GachaResultSlot>();
    private Coroutine revealCo;
    private bool isRevealing;

    private void Awake()
    {
        if (confirmButton)
        {
            confirmButton.onClick.AddListener(Close);
            confirmButton.interactable = false; // 연출 끝나고 true
        }

        if (skipArea)
        {
            skipArea.onClick.RemoveAllListeners();
            skipArea.onClick.AddListener(SkipReveal);
            skipArea.gameObject.SetActive(false); // 연출 때만 활성
        }
    }

    public void OpenAndShow(List<GachaData> results)
    {
        gameObject.SetActive(true);
        confirmButton.interactable = false;
        ClearGrid();

        // 최대 10개만 표시(원하면 제거)
        int count = Mathf.Min(10, results.Count);

        // 미리 슬롯 생성(숨김)
        for (int i = 0; i < count; i++)
        {
            var slot = Instantiate(slotPrefab, grid);
            slot.gameObject.SetActive(true);
            slot.group.alpha = 0f;
            slot.transform.localScale = Vector3.one * 0.85f;
            slot.SetData(results[i]);
            spawned.Add(slot);
        }

        // 순차 등장 코루틴 시작
        if (revealCo != null) StopCoroutine(revealCo);
        revealCo = StartCoroutine(Co_Reveal());
    }

    private IEnumerator Co_Reveal()
    {
        isRevealing = true;
        if (skipArea) skipArea.gameObject.SetActive(true);

        for (int i = 0; i < spawned.Count; i++)
        {
            // 슬롯 개별 등장
            yield return spawned[i].Reveal(popAnimTime);

            // 다음 슬롯까지 간격
            float t = 0f;
            while (t < revealInterval && isRevealing) { t += Time.deltaTime; yield return null; }
            if (!isRevealing) break; // 스킵되면 빠져나감
        }

        // 스킵이 눌렸거나 마지막까지 다 끝난 경우: 나머지 전부 즉시 표시
        for (int i = 0; i < spawned.Count; i++)
        {
            var s = spawned[i];
            s.group.alpha = 1f;
            s.transform.localScale = Vector3.one;
        }

        isRevealing = false;
        if (skipArea) skipArea.gameObject.SetActive(false);
        confirmButton.interactable = true;
    }

    // 연출 스킵 (패널 아무 데나 탭해서 전체 공개)
    public void SkipReveal()
    {
        if (!isRevealing) return;
        isRevealing = false; // Co_Reveal에서 나머지를 한 번에 표출
    }

    public void Close()
    {
        gameObject.SetActive(false);
        ClearGrid();
    }

    private void ClearGrid()
    {
        if (revealCo != null) { StopCoroutine(revealCo); revealCo = null; }
        isRevealing = false;
        foreach (Transform c in grid) Destroy(c.gameObject);
        spawned.Clear();
    }
}
