using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.OnHealthChanged += UpdateHealthUI;
            player.OnDeath += HideHealthUI;
        }
        
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHealthUI;
            player.OnDeath -= HideHealthUI;
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (healthSlider != null)
            healthSlider.value = current / max;
    }

    private void HideHealthUI()
    {
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);
    }
}
