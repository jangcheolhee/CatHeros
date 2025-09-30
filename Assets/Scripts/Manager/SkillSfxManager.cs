using UnityEngine;

public class SkillSfxManager : MonoBehaviour
{
    public static SkillSfxManager Instance;

    public AudioSource[] sources; // 미리 여러 개 준비
    private int index = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ?? 씬 전환해도 AudioSource 유지
        }
        else Destroy(gameObject);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sources[index].PlayOneShot(clip);
        index = (index + 1) % sources.Length; // 다음 소스 선택 (라운드 로빈)
    }
}
