using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI speed;
    public TextMeshProUGUI autoText;
    [System.Serializable]
    public class PanelEntry
    {
        public string name;
        public GameObject panel;
    }

   
    public List<PanelEntry> panels;

    private Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();
    private bool timeSpeed = false;

    private void Awake()
    {
        foreach (var entry in panels)
        {
            if (entry.panel != null)
            {
                panelDict[entry.name] = entry.panel;
                entry.panel.SetActive(false); 
            }
        }
    }
    public void ShowPanel(string name, bool pauseGame = false)
    {
       
        foreach (var p in panelDict.Values)
            p.SetActive(false);

        
        if (panelDict.ContainsKey(name))
            panelDict[name].SetActive(true);

        
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
            HideAllPanels();
        }
        else
        {
            ShowPanel("PausePanel", true);
        }
    }
    public void ShowEndPanel() => ShowPanel("EndPanel", true);

    public void ResumeGame() => HideAllPanels();
    public void SpeedChange()
    {
        timeSpeed = !timeSpeed;
        if(timeSpeed)
        {
            Time.timeScale = 2f;
            speed.text = "2배속";
        }
        else
        {
            Time.timeScale = 1f;
            speed.text = "1배속";
        }
    }
    public void ChageAutoText(bool auto)
    {
        if(auto)
        {
            autoText.text = "자동";
        }
        else
        {
            autoText.text = "수동";
        }
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterSelect");
    }
}
