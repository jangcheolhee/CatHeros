using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SkillButton : MonoBehaviour
{
    
    public Button button;
    public Image cooldownOverlay;        
    public TextMeshProUGUI cooldownText; 

    private Player player;  
    private float cooldown;
    private bool isCoolingDown = false;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        ResetCooldownUI();
    }

    public void Setup(Player p, float skillCooldown)
    {
        player = p;
        cooldown = skillCooldown;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSkill);
    }

    private void OnClickSkill()
    {
        if (isCoolingDown || player == null) return;

        player.UseSkill();
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        button.interactable = false;

        float timer = cooldown;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = timer / cooldown;

            if (cooldownText != null)
                cooldownText.text = Mathf.Ceil(timer).ToString();

            yield return null;
        }

        ResetCooldownUI();
    }

    private void ResetCooldownUI()
    {
        isCoolingDown = false;
        button.interactable = true;

        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = 0f;

        if (cooldownText != null)
            cooldownText.text = "";
    }
}
