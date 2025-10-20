using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainScene : GenericWindow
{
    public TextMeshProUGUI gold;
    public TextMeshProUGUI exp;
    public TextMeshProUGUI name;
    public TextMeshProUGUI chur;
    public TMP_InputField nameField;
    public GameObject userInfo;
    private float timer;
    private void Start()
    {
         if (GameManager.Instance.WindowToOpenOnReturn != Windows.Main)
        {
            manager.Open(GameManager.Instance.WindowToOpenOnReturn);
            GameManager.Instance.WindowToOpenOnReturn = Windows.Main; // ÃÊ±âÈ­
        }
        GameManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
        gold.text = GameManager.Instance.Gold.ToString();
        exp.text = GameManager.Instance.Exp.ToString();
        name.text = GameManager.Instance.Name;
    }
    public override void Open()
    {
        AudioManager.Instance.PlayMainBgm();
        userInfo.SetActive(false);
        base.Open();
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>($"Effects/11306"));
            go.transform.position = new Vector3(Random.Range(-2, 2), Random.Range(-3, 3),0);
            Destroy(go, 3);
            timer = 0;
        }
    }
    void UpdateCurrencyUI()
    {
       
        gold.text = GameManager.Instance.Gold.ToString();
        exp.text = GameManager.Instance.Exp.ToString();
        name.text = GameManager.Instance.Name.ToString();
        chur.text = GameManager.Instance.Chur.ToString();
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
    public void OnClickConfirm()
    {
        GameManager.Instance.Name = nameField.text;
        nameField.text = "";
        GameManager.Instance.Save();
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

}
