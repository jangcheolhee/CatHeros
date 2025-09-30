using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSelectScene : GenericWindow
{

    public StageCheckPanel checkPanel;
    public ScrollRect scrollRect;
    public Transform content;
    public GameObject stageButtonPrefab;
    public Button startButton;
    public TextMeshProUGUI titleText;



    private readonly List<StageButton> items = new();
    private int selectedStageId = -1;

    public override void Open()
    {
        titleText?.SetText("Stage Select");
        Populate();
        OnClickBack();
        AudioManager.Instance.PlayStageSelectBgm();
        base.Open();

    }


    void Populate()
    {
        // 기존 정리
        foreach (Transform child in content) Destroy(child.gameObject);
        items.Clear();

        int count = 0;
        foreach (var s in DataTableManger.StageTable.Table)
        {
            var go = Instantiate(stageButtonPrefab, content);
            var view = go.GetComponent<StageButton>();

            if (count > GameManager.Instance.ClearStage)
            {
                view.Init(s.Value, true);
            }
            else
            {
                view.Init(s.Value, false);
            }

            var btn = go.GetComponent<Button>();

            int id = s.Key;
            btn.onClick.AddListener(() => OnClickStage(id));
            if (count > GameManager.Instance.ClearStage)
            {
                btn.interactable = false;
            }
            items.Add(view);
            count++;
        }
    }

    void OnClickStage(int id)
    {
        selectedStageId = (selectedStageId == id) ? -1 : id;
        GameManager.Instance.SelectedStageId = selectedStageId;
        //manager.Open(Windows.Character);
        checkPanel.Show();

    }


    public void OnClickCollection()
    {
        
        manager.Open(Windows.Collection);
    }
    public void OnClickHome()
    {

       
        manager.Open(Windows.Main);
    }
    public void OnClickBack()
    {
        checkPanel.Hide();
        selectedStageId = -1;

    }
    public void OnClickStart()
    {
        
        manager.Open(Windows.Character);
    }
}
