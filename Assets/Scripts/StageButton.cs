using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{

    public Image thumbnail;
    public TextMeshProUGUI nameText;



    public int stageId;

    public void Init(StageData data, bool clear)
    {

        stageId = data.StageID;

        if (!clear)
            thumbnail.sprite = Resources.Load<Sprite>($"icon/Stage{Random.Range(0,12)}");
        else
        {
            thumbnail.sprite = Resources.Load<Sprite>("icon/Lock");
        }

            nameText.text = data.StageName;


    }


}