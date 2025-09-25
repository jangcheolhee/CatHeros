using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class StageCheckPanel : MonoBehaviour
{
    public TextMeshProUGUI stage;
    public GameObject rewardPrefab;
    public Transform contentParent;

    
    public void Show()
    {
        gameObject.SetActive(true);
        UpdatePanel();
       
    }
 public void UpdatePanel()
    {
         foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    var rewardDatas = DataTableManger.RewardTable.Get(GameManager.Instance.SelectedStageId);
        



        foreach (var data in rewardDatas)
        {
            
            GameObject item = Instantiate(rewardPrefab, contentParent);
    Image[] images = item.GetComponentsInChildren<Image>(true);
    var image = images[0];
            foreach (var img in images)
            {
          

               
                if (img.gameObject.name == "Icon")
                {
                    img.sprite = Resources.Load<Sprite>($"icon/{data.R_item_id}");
                    Debug.Log(data.R_item_id);

                    break;
                }
            }
            
            var text = item.GetComponentInChildren<TextMeshProUGUI>();
text.text = data.quantity.ToString();
            //text.text = DataTableManger.ItemTable.Get(data.R_item_id).Item_Name;


        }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
