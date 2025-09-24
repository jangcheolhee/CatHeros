using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionScene : MonoBehaviour
{
    
    public void OnClickHome()
    {
        SceneManager.LoadScene("Main");
    }
    public void OnClickCollection()
    {
        SceneManager.LoadScene("Collection");
    }
}
