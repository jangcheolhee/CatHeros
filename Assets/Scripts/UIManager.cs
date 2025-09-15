using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [System.Serializable]
    public class PanelEntry
    {
        public string name;
        public GameObject panel;
    }

    [Header("Panel List")]
    public List<PanelEntry> panels;

    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var entry in panels)
        {
            if (entry.panel != null)
            {
                panelDict[entry.name] = entry.panel;
                entry.panel.SetActive(false); // 시작 시 숨김
            }
        }
    }

    public void ShowPanel(string name, bool pauseGame = false)
    {
        // 모든 패널 끄고
        foreach (var p in panelDict.Values)
            p.SetActive(false);

        // 특정 패널 켜기
        if (panelDict.ContainsKey(name))
            panelDict[name].SetActive(true);

        // 일시정지 여부 반영
        Time.timeScale = pauseGame ? 0f : 1f;
    }

    public void HideAllPanels()
    {
        foreach (var p in panelDict.Values)
            p.SetActive(false);

        Time.timeScale = 1f;
    }



    public void TogglePausePanel()
    {
        if (!panelDict.ContainsKey("PausePanel")) return;

        bool isActive = panelDict["PausePanel"].activeSelf;

        if (isActive)
        {
            HideAllPanels(); // 이미 켜져있으면 닫기
        }
        else
        {
            ShowPanel("PausePanel", true); // 꺼져있으면 켜기
        }
    }
    public void ShowEndPanel() => ShowPanel("EndPanel", true);

    public void ResumeGame() => HideAllPanels();

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
