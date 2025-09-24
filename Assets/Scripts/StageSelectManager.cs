using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
  
    public ScrollRect scrollRect;       
    public Transform content;          
    public GameObject stageButtonPrefab;
    public Button startButton;
    public TextMeshProUGUI titleText;

         

    private readonly List<StageButton> items = new();
    private int selectedStageId = -1;

    void Start()
    {
        titleText?.SetText("Stage Select");
        Populate();
        
    }

    void Populate()
    {
        // 기존 정리
        foreach (Transform child in content) Destroy(child.gameObject);
        items.Clear();

        
        foreach (var s in DataTableManger.StageTable.Table)
        {
            var go = Instantiate(stageButtonPrefab, content);
            var view = go.GetComponent<StageButton>();
            bool isSelected = (s.Key == selectedStageId);
            view.Bind(s.Value, isSelected);

            var btn = go.GetComponent<Button>();
            int id = s.Key;
            btn.onClick.AddListener(() => OnClickStage(id));

            items.Add(view);
        }
    }

    void OnClickStage(int id)
    {
        selectedStageId = (selectedStageId == id) ? -1 : id;
        GameManager.Instance.SelectedStageId = selectedStageId;   
        SceneManager.LoadScene("CharacterSelect");
       
    }


 
    public void OnClickCollection()
    {
        SceneManager.LoadScene("Collection");
    }
    public void OnClickHome()
    {
        
        
        SceneManager.LoadScene("Main");           
    }
}
