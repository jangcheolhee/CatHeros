using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public List<Slider> playerHpSliders;
    public List<SkillButton> skillButtons;   
    public BattleManager battleManager;
    public TextMeshProUGUI waveText;

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
            {
                var player = battleManager.Players[i];
                int index = i;

                
                float skillCooldown = player.skillData.Cooldown;
                skillButtons[index].Setup(player, skillCooldown);

                
                player.OnHealthChanged += (current, max) =>
                {
                    playerHpSliders[index].value = current / max;
                };

                
                player.OnDeath += () =>
                {
                    playerHpSliders[index].gameObject.SetActive(false);
                    skillButtons[index].gameObject.SetActive(false);
                };

               
                playerHpSliders[index].value = player.CurrentHP / player.MaxHP;
            }
            else
            {
                playerHpSliders[i].gameObject.SetActive(false);
                skillButtons[i].gameObject.SetActive(false);
            }
        }
    }
}
