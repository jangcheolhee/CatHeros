using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionScene : GenericWindow
{
    
    public void OnClickHome()
    {
        manager.Open(Windows.Main);
    }
    public void OnClickStage()
    {
        manager.Open(Windows.Stage);
    }
    

}
