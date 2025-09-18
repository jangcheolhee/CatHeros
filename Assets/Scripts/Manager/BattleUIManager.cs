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
        int f = 0;
        int r = 0;
        for (int i = 0; i < playerHpSliders.Count; i++)
        {
            if (i < battleManager.PlayerCount)
            {
                int index = i;
                Player player;
                if(f < battleManager.Players[FormationRow.Front].Count)
                {
                    player = battleManager.Players[FormationRow.Front][f++];
                }
                else
                {
                    player = battleManager.Players[FormationRow.Rear][r++];
                }



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
