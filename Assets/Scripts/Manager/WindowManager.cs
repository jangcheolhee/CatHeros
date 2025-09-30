using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public List<GenericWindow> windows;
    public Windows defaultWindow;
    public Windows prevWindow;
    

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
        //모달 모달리스
        windows[(int)CurrentWindow].Close();
        prevWindow = CurrentWindow;
        CurrentWindow = id;
       
        windows[(int)CurrentWindow].Open();
       
        
    }

    public void OnClickCoin()
    {
        GameManager.Instance.Gold = 999999;
        GameManager.Instance.Exp = 999999;
    }


}
