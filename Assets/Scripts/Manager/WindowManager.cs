using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public List<GenericWindow> windows;
    public Windows defaultWindow;
    public Windows prevWindow;

    public Button btn;
    public TextMeshProUGUI btnText;
    public Image btnImage;
    public Windows CurrentWindow { get; private set; }
    private void Start()
    {
        foreach (var window in windows)
        {
            window.Init(this);
            //window.Close();
            window.gameObject.SetActive(false);
        }
        prevWindow = defaultWindow;
        CurrentWindow = defaultWindow;
        windows[(int)CurrentWindow].Open();
    }
    public void Open(Windows id)
    {
        
        windows[(int)CurrentWindow].Close();
        prevWindow = CurrentWindow;
        CurrentWindow = id;

        windows[(int)CurrentWindow].Open();

        if (CurrentWindow == Windows.Main)
        {
            btnText.text = "나가기";
            btnImage.sprite = Resources.Load<Sprite>("icon/Exit");

        }
        else
        {
            btnText.text = "홈";
            btnImage.sprite = Resources.Load<Sprite>("icon/Home");
        }
        

       

    }
    public void OnClick()
    {
        if(CurrentWindow == Windows.Main)
        {
            Debug.Log(1111);
            Application.Quit();
        }
        else
        {
            Debug.Log(213435);
            Open(Windows.Main);
        }
    }

    public void OnClickCoin()
    {
        //#if UNITY_EDITOR
        GameManager.Instance.Gold = 9999999;
        GameManager.Instance.Exp = 9999999;
        GameManager.Instance.Chur = 9999;
        GameManager.Instance.Save();
        //#endif
    }

}
