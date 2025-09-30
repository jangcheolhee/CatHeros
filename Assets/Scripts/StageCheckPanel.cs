using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageCheckPanel : MonoBehaviour
{
    public TextMeshProUGUI stage;
    public GameObject rewardPrefab;
    public Transform contentParent;
    public TextMeshProUGUI text;


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
        text.text = $"Stage {DataTableManger.StageTable.Get(GameManager.Instance.SelectedStageId).StageName}";



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
    public void UpdateLosePanel()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        var rewardDatas = DataTableManger.RewardTable.Get(GameManager.Instance.SelectedStageId);
        text.text = $"Stage {DataTableManger.StageTable.Get(GameManager.Instance.SelectedStageId).StageName}";



        foreach (var data in rewardDatas)
        {
            if (data.condition == "스테이지 클리어")
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
    }
    public void GetItems(bool get)
    {
        var rewardDatas = DataTableManger.RewardTable.Get(GameManager.Instance.SelectedStageId);

        foreach (var data in rewardDatas)
        {
            if (!get && data.condition != "스테이지 클리어") continue;
            switch (data.R_item_id)
            {
                case 40501:

                    GameManager.Instance.Chur += data.quantity;

                    break;
                case 40502:

                    GameManager.Instance.Gold += data.quantity;
                    break;

                case 20503:

                    GameManager.Instance.Yarn += data.quantity;

                    break;
                case 40504:

                    GameManager.Instance.Exp += data.quantity;

                    break;

            }
        }
    }



    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
