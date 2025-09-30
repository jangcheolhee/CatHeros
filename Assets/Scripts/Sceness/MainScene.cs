using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : GenericWindow
{
    public TextMeshProUGUI gold;
    public TextMeshProUGUI yarn;
    public GameObject userInfo;
    private void Start()
    {
         if (GameManager.Instance.WindowToOpenOnReturn != Windows.Main)
        {
            manager.Open(GameManager.Instance.WindowToOpenOnReturn);
            GameManager.Instance.WindowToOpenOnReturn = Windows.Main; // √ ±‚»≠
        }
        GameManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
        gold.text = GameManager.Instance.Gold.ToString();
        yarn.text = GameManager.Instance.Yarn.ToString();
    }
    public override void Open()
    {
        AudioManager.Instance.PlayMainBgm();
        userInfo.SetActive(false);
        base.Open();
    }
    void UpdateCurrencyUI()
    {
       
        gold.text = GameManager.Instance.Gold.ToString();
    }
    public void OnClickBattle()
    {
        manager.Open(Windows.Stage);
    }
    public void OnClickHome()
    {
        manager.Open(Windows.Main);
    }
    public void OnClickCollection()
    {
        manager.Open(Windows.Collection);
    }
    public void OnClickGacha()
    {
        manager.Open(Windows.Gacha);
    }
    public void OpenInfo()
    {
        userInfo.SetActive(true);
    }
    public void CloseInfo()
    {
        userInfo.SetActive(false);
    }


}
