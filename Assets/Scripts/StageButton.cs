using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
   
    public Image thumbnail;
    public TextMeshProUGUI nameText;
    

    public int stageId;

    public void Bind(StageData data, bool selected)
    {

        stageId = data.StageID;
        nameText.text = data.StageName;
        
       
    }

  
}