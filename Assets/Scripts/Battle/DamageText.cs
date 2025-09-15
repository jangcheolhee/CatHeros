using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 2f;  
    public float duration = 1f;    
    private TextMeshProUGUI text;
    private Color startColor;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        startColor = text.color;
    }

    public void SetText(int damage)
    {
        text.text = damage.ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            float alpha = duration / 1f; 
            text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
    }
}
