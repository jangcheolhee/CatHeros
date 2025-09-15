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
    public TextMeshProUGUI waveText;   // Inspector�� ����

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

                // HP ��ȭ �̺�Ʈ ����
                player.OnHealthChanged += (current, max) =>
                {
                    playerHpSliders[index].value = current / max;
                };

                // �׾��� �� HP �� ��Ȱ��ȭ
                player.OnDeath += () =>
                {
                    playerHpSliders[index].gameObject.SetActive(false);
                };

                // �ʱ�ȭ (Setup �� �� ���߱�)
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
