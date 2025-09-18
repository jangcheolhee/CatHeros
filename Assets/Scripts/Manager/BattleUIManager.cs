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
    public TextMeshProUGUI timerText;
    private Sprite spriteIcon;

    public void UpdateWaveText(int current, int total)
    {
        if (waveText != null)
            waveText.text = $"Wave {current}/{total}";
    }

    private void Start()
    {
        battleManager.OnTimeChanged += UpdateTimerUI;
        
        for (int i = 0; i < playerHpSliders.Count; i++)
        {
            if (i < battleManager.Players.Count)
            {
                var player = battleManager.Players[i];
                int index = i;


                float skillCooldown = player.SkillData.Cooldown;
                skillButtons[index].Setup(player, skillCooldown);
                spriteIcon = Resources.Load<Sprite>($"Icon/{player.characterData.Character_ID}");
                skillButtons[index].skillText.text = $"{player.characterData.Name}\n{player.SkillData.Skill_Name}";
                skillButtons[index].icon.sprite = spriteIcon;
                player.OnHealthChanged += (current, max) =>
                {
                    playerHpSliders[index].value = current / max;
                };


                player.OnDeath += () =>
                {
                    playerHpSliders[index].gameObject.SetActive(false);
                    //skillButtons[index].gameObject.SetActive(false);
                    skillButtons[index].button.interactable = false;
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
    private void UpdateTimerUI(float remain)
    {
        int minutes = Mathf.FloorToInt(remain / 60f);
        int seconds = Mathf.FloorToInt(remain % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
