using UnityEngine;
using UnityEngine.UI;

public class LabeledSliderRow : MonoBehaviour
{
    public Image icon;
    public Toggle toggle;    // Mute ¿ë
    public Slider slider;    // 0~1

    public void Setup(Sprite iconSprite, float value, bool isOn,
                      System.Action<bool> onToggle,
                      System.Action<float> onSlider)
    {
        if (icon) icon.sprite = iconSprite;
        if (toggle)
        {
            toggle.isOn = isOn;
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(v => onToggle?.Invoke(v));
        }
        if (slider)
        {
            slider.minValue = 0f; slider.maxValue = 1f;
            slider.value = value;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(v => onSlider?.Invoke(v));
        }
    }

    public void SetInteractable(bool enabled)
    {
        slider.interactable = enabled;
        var cg = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        cg.alpha = enabled ? 1f : 0.5f;
    }
}
