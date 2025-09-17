using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Slider healthSlider;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged += UpdateHealthUI;
            enemy.OnDeath += HideHealthUI;
        }
    }

    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged -= UpdateHealthUI;
            enemy.OnDeath -= HideHealthUI;
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
    public void Refresh()
    {
        if (enemy != null)
            UpdateHealthUI(enemy.CurrentHP, enemy.MaxHP);
    }
}
