using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public List<GenericWindow> windows;
    public Windows defaultWindow;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;


    public Windows CurrentWindow { get; private set; }
    private void Start()
    {
        foreach (var window in windows)
        {
            window.Init(this);
            //window.Close();
            window.gameObject.SetActive(false);
        }
        CurrentWindow = defaultWindow;
        windows[(int)CurrentWindow].Open();
    }
    public void Open(Windows id)
    {
        //모달 모달리스
        windows[(int)CurrentWindow].Close();

        CurrentWindow = id;
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        switch (CurrentWindow)
        {

            case Windows.Main:
                {
                    button1.onClick.AddListener(() => Open(Windows.Stage));

                    var text = button1.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "배틀";
                    button2.onClick.AddListener(() => Open(Windows.Collection));

                }
                break;
            case Windows.Stage:
                {
                    button1.onClick.AddListener(() => Open(Windows.Main));

                    var text = button1.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "홈";
                    button2.onClick.AddListener(() => Open(Windows.Collection));

                }
                break;
            case Windows.Collection:
                {
                    button1.onClick.AddListener(() => Open(Windows.Main));
                    var text = button1.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "홈";
                    button2.onClick.AddListener(() => Open(Windows.Stage));

                }
                break;
            case Windows.Character:
                {
                    button1.onClick.AddListener(() => Open(Windows.Stage));
                    var text = button1.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "홈";
                    button2.onClick.AddListener(() => Open(Windows.Collection));

                }
                break;
        }
        windows[(int)CurrentWindow].Open();
       
        
    }
   
}
