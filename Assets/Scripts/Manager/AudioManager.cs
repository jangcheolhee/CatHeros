using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer & UI")]
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    [Header("BGM Sources & Clips")]
    public AudioSource bgmSource;
    public AudioClip mainBgm;
    public AudioClip gachaBgm;
    public AudioClip stageSelectBgm;
    public AudioClip BattleBgm;

    const float MIN = 0.0001f; // log10(0)은 -무한대 → 아주 작은 값으로 대체

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // ?? 저장된 설정 불러오기
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1f);
        muteToggle.isOn = PlayerPrefs.GetInt("Muted", 0) == 1;

        // ?? 불러온 값 적용
        ApplyMusic(musicSlider.value);
        ApplySFX(sfxSlider.value);
        ApplyMute(muteToggle.isOn);

        // ?? UI 이벤트 등록
        musicSlider.onValueChanged.AddListener(ApplyMusic);
        sfxSlider.onValueChanged.AddListener(ApplySFX);
        muteToggle.onValueChanged.AddListener(ApplyMute);
    }

    #region Mixer Volume
    public void ApplyMusic(float musicVolume)
    {
        SetVolume("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);
    }

    public void ApplySFX(float sfxVolume)
    {
        SetVolume("SFXVol", sfxVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
    }

    public void ApplyMute(bool isMuted)
    {
        if (isMuted)
            mixer.SetFloat("MasterVol", -80f);
        else
            mixer.SetFloat("MasterVol", 0f);

        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void SetVolume(string name, float volume)
    {
        float value = Mathf.Clamp(volume, MIN, 1f);
        float dB = Mathf.Log10(value) * 20f;
        mixer.SetFloat(name, dB);
    }
    #endregion

    #region BGM Control
    public void PlayBgm(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;

        if (bgmSource.clip == clip) return; // 같은 곡이면 무시

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // 편의 메서드
    public void PlayMainBgm() => PlayBgm(mainBgm);
    public void PlayGachaBgm() => PlayBgm(gachaBgm);
    public void PlayStageSelectBgm() => PlayBgm(stageSelectBgm);
    public void PlayBattleBgm() => PlayBgm(BattleBgm);
    #endregion
}
