using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPanel : MonoBehaviour
{
    
    public GameObject enemyInfoPrefab;   
    public Transform contentParent;        
    public Button closeButton;           

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        

        
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        var stageData = DataTableManger.StageTable.Get(GameManager.Instance.SelectedStageId);
        
        var Waves = DataTableManger.WaveTable.Get(stageData.StageID);
        List<int> check = new();
        for(int i = 0; i < stageData.MaxWaveCount; i++)
        {
            var waveq = Waves[i];
            foreach (var enemys in waveq.Enemies)
            {
                var data = enemys.Monster_ID;
                if(!check.Contains(data))
                {
                    check.Add(data);
                    GameObject item = Instantiate(enemyInfoPrefab, contentParent);
                    var image = item.GetComponentInChildren<Image>();
                    image.sprite = Resources.Load<Sprite>($"icon/{data}");
                    var texts = item.GetComponentsInChildren<TextMeshProUGUI>();
                    texts[0].text = DataTableManger.MonsterTable.Get(data).M_Name;
                    texts[1].text = DataTableManger.SkillTable.Get(DataTableManger.MonsterTable.Get(data).M_Skill_Set_ID).Skill_Name;
                }
                
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
