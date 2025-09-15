using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public List<Slider> playerHpSliders;
    public List<Button> skillButtons;
    public BattleManager battleManager;
    public TextMeshProUGUI waveText;   // Inspector에 연결

    public void UpdateWaveText(int current, int total)
    {
        if (waveText != null)
            waveText.text = $"Wave {current}/{total}";
    }
    private void Start()
    {
        for (int i = 0; i < playerHpSliders.Count; i++)
        {
           
            
            if (i < battleManager.Players.Count)
            {   var player = battleManager.Players[i];
                int index = i;

                skillButtons[index].onClick.AddListener(() => player.UseSkill());

                // HP 변화 이벤트 구독
                player.OnHealthChanged += (current, max) =>
                {
                    playerHpSliders[index].value = current / max;
                };

                // 죽었을 때 HP 바 비활성화
                player.OnDeath += () =>
                {
                    playerHpSliders[index].gameObject.SetActive(false);
                };

                // 초기화 (Setup 후 값 맞추기)
                playerHpSliders[index].value = player.CurrentHP / player.MaxHP;
            }
            else
            {
                playerHpSliders[i].gameObject.SetActive(false);
                skillButtons[i].interactable = false;
            }
        }
    }
}
