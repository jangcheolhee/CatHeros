using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public AudioClip click;
    void Start()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
Debug.unityLogger.logEnabled = false;
#endif

    }
    public void OnClickButton()
    {
        SkillSfxManager.Instance.PlaySfx(click);
        SceneManager.LoadScene("Main");
    }
    
}
