using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GachaResultSlot : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
   
    public CanvasGroup group;

    private void Reset()
    {
        group = GetComponent<CanvasGroup>();
        if (!group) group = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetData(GachaData data)
    {
        // 아이콘: Result_ID 기준 리소스 로드 (없으면 플레이스홀더)
        var spr = Resources.Load<Sprite>($"icon/{data.Result_ID}");
        icon.sprite = spr ? spr : Resources.Load<Sprite>("icon/placeholder");

        // 이름 포맷팅
        if (data.Gacha_Type == 1)
        {
            // 캐릭터: "N 토토" 같은 형식
            nameText.text = $"{RarityToString(data.Rarity)} {data.Reward_Name}";
        }
        else
        {
            // 재화류: "경험치 800", "츄르 1" 등
            string amount = data.Amount > 1 ? $" {data.Amount}" : string.Empty;
            nameText.text = $"{data.Reward_Name}{amount}";
        }

    
    }

    public IEnumerator Reveal(float popTime = 0.15f)
    {
        // 등장 연출: 알파 페이드 + 약간의 팝 스케일
        transform.localScale = Vector3.one * 0.85f;
        group.alpha = 0f;

        float t = 0f;
        while (t < popTime)
        {
            t += Time.deltaTime;
            float k = t / popTime;
            group.alpha = Mathf.Lerp(0f, 1f, k);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.85f, Vector3.one, k);
            yield return null;
        }

        group.alpha = 1f;
        transform.localScale = Vector3.one;
    }

    private string RarityToString(int r)
    {
        switch (r) { case 1: return "N"; case 2: return "R"; case 3: return "SR"; case 4: return "SSR"; }
        return "";
    }
}
