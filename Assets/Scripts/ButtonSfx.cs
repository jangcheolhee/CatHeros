using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSfx : MonoBehaviour
{
    public AudioClip clickSfx;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SkillSfxManager.Instance.PlaySfx(clickSfx);
        });
    }
}
