using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : GenericWindow
{
    public TextMeshProUGUI gold;
    public TextMeshProUGUI yarn;
    private void Start()
    {
        gold.text = GameManager.Instance.Gold.ToString();
        yarn.text = GameManager.Instance.Yarn.ToString();
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
}
